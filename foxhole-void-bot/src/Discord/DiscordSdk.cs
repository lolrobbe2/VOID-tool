using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable

namespace FoxholeBot.src.Discord
{
    public class DiscordSDK
    {
        private HttpClient client { get; set; }
        private NavigationManager navigation { get; set; }
        private IJSRuntime js { get; set; }
        private DiscordActivityBridge bridge { get; set; }

        private IDictionary<string, Task> pendingCommands;
        public DiscordSDK(IJSRuntime js, HttpClient client, NavigationManager navigation)
        {
            this.client = client;
            this.js = js;
            this.navigation = navigation;
            this.bridge = new DiscordActivityBridge(js);
        }
        [JSInvokable]
        public static void ReceiveDiscordMessage(object data)
        {
            // data is the message received via postMessage
            Console.WriteLine("Discord message received:");
            Console.WriteLine(data);

        }
        Task SendCommand(object payload)
        {
            string nonce =  Guid.NewGuid().ToString();  
            pendingCommands.Add(nonce,bridge.PostCommandAsync(Opcodes.FRAME, payload, nonce));

            if (pendingCommands.TryGetValue(nonce, out Task command))
            {
                return command;
            } 
            else 
            {
                return Task.CompletedTask;
            }
        }
    }
}
