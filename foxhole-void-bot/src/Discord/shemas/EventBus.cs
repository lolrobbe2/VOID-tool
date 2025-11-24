using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoxholeBot.src.Discord.shemas
{
    public class EventData<TPayload>
    {
        public string name { get; set; } = string.Empty;
        public TPayload payload { get; set; } = default!;
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
        public async Task on(string name, Func<object, Task> handler) 
        {
            events[name] = handler;
            await RegisterListener(name);
        }

        public void off(string name)
        {
            events.Remove(name);    
        }

        public void call(EventData<object> data) {
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
            EventData<JsonElement> evt = JsonSerializer.Deserialize<EventData<JsonElement>>(data.GetRawText());

            foreach (var eventBus in eventBusses)
            {
                eventBus.Value.call(evt);
            }
        }
    }
}
