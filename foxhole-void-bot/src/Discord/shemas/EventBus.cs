using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoxholeBot.src.Discord.shemas
{

    public class EventData<TPayload>
    {
        [JsonConstructor]
        public EventData(JsonElement element)
        {
            name = element[0].ToString();
            payload = JsonSerializer.Deserialize<TPayload>(element[1]);
            guid = Guid.Parse(element[2].ToString());
        }
        public string name { get; set; } = string.Empty;
        public TPayload payload { get; set; } = default!;
        public Guid guid { get; set; } = Guid.Empty;
    }
    public class EventBus
    {
        private static Dictionary<Guid, EventBus> eventBusses = new();

        private Dictionary<string, Func<object, Task>> events = new Dictionary<string, Func<object, Task>>(); 
        private readonly IJSRuntime _jsr;
        private readonly Guid guid = Guid.NewGuid();
        public EventBus(IJSRuntime jsr) 
        {
            _jsr = jsr;
            eventBusses[guid] = this;
        }
        ~EventBus()
        {
            eventBusses.Remove(guid);
        }
        public async Task on(string name, Func<object, Task> handler) 
        {
            events[name] = handler;
            await RegisterListener(name);
        }

        public void off(string name)
        {
            events.Remove(name);    
        }
        
        public void call<T>(EventData<T> data) {
            if(events.TryGetValue(data.name, out var action))
            {
                action(data.payload);
            }
        }
        /// <summary>
        /// This fucntion registers and eventListener on the Web/Javascript side and will glue/bind it to the internal ReceiveEvent function
        /// </summary>
        /// <param name="name">name of the event</param>
        /// <returns></returns>
        private async Task RegisterListener(string name) 
        {
            // Get the current assembly name dynamically
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

            string js = $@"
                window.addEventListener('{name}', function(event) {{
                    const eventstruct = ['{name}',event.data,'{guid.ToString()}'];
                    DotNet.invokeMethodAsync('{assemblyName}', 'ReceiveEvent', eventstruct);
                }});
            ";

            await _jsr.InvokeVoidAsync("eval", js);
        }

        /// <summary>
        /// This function is called when the eventBus receives an event.
        /// </summary>
        /// <param name="data"></param>
        [JSInvokable]
        public static void ReceiveEvent(JsonElement data)
        {
            Console.WriteLine("message received");
            Console.WriteLine(data.GetRawText());
            EventData<JsonElement> evt = new EventData<JsonElement>(data);

            if (eventBusses.TryGetValue(evt.guid, out EventBus bus)) {
                bus.call(evt);
            }
        }
    }
}
