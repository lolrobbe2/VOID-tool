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
        private static DiscordSDK? SingletonSDK { get; set; }

        private HttpClient client { get; set; }
        private NavigationManager navigation { get; set; }
        private IJSRuntime js { get; set; }
        private DiscordActivityBridge bridge { get; set; }

        IDictionary<string, TaskCompletionSource<object>> pendingCommands = new Dictionary<string, TaskCompletionSource<object>>();
        public DiscordSDK(IJSRuntime js, HttpClient client, NavigationManager navigation)
        {
            this.client = client;
            this.js = js;
            this.navigation = navigation;
            this.bridge = new DiscordActivityBridge(js);
            SingletonSDK = this;
        }
        public void OnMessage(object data)
        {
            var taskSource = pendingCommands["nonce here"];
        }
        [JSInvokable]
        public static void ReceiveDiscordMessage(object data)
        {
            // data is the message received via postMessage
            Console.WriteLine("Discord message received:");
            SingletonSDK.OnMessage(data);
        }

        /// <summary>
        /// this function sends a commands to the bridge and then ackowladges it
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        async Task<object> SendCommandAsync(object payload)
        {
            string nonce = Guid.NewGuid().ToString();

            // PostCommandAsync should return Task<T>
            var taskSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            await bridge.PostCommandAsync(Opcodes.FRAME, payload, nonce);

            pendingCommands.Add(nonce, taskSource);
            return await taskSource.Task;
        }
    }
}
