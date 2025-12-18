using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoxholeBot.src.Discord.shemas
{
    public class EventData<TPayload>
    {
        public string name { get; set; }
        public TPayload payload { get; set; }
    }
    public class EventBus
    {
        private static Dictionary<Guid, EventBus> eventBusses = new();

        private Dictionary<string, Func<JsonElement, Task>> events = new Dictionary<string, Func<JsonElement, Task>>(); 
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
        public async Task on(string name, Func<JsonElement, Task> handler) 
        {
            events[name] = handler;
            await RegisterListener(name);
        }

        public void off(string name)
        {
            events.Remove(name);    
        }

        public void call(EventData<JsonElement> data) {
            if(events.TryGetValue(data.name, out var action))
            {
                action(data.payload);
            }
        }
        private async Task RegisterListener(string name) 
        {
            // Get the current assembly name dynamically
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

            string js = $@"
                window.addEventListener('{name}', function(event) {{
                    const eventstruct = ['{name}',event.data];
                    DotNet.invokeMethodAsync('{assemblyName}', 'ReceiveEvent', eventstruct);
                }});
            ";

            await _jsr.InvokeVoidAsync("eval", js);
        }

        [JSInvokable]
        public static void ReceiveEvent(JsonElement data)
        {
            Console.WriteLine("message received");
            EventData<JsonElement> evt = new() { name = data[0].ToString(), payload = data[1] };
        

            foreach (var eventBus in eventBusses)
            {
                if (eventBus.Key == Guid.Empty)
                    continue;
                eventBus.Value.call(evt);
            }
        }
    }
}
