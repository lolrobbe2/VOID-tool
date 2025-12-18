using DocumentFormat.OpenXml.Wordprocessing;
using FoxholeBot.src.Discord.shemas;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#nullable enable

namespace FoxholeBot.src.Discord
{
    public struct HandshakePayload
    {
        [JsonPropertyName("v")]
        public int V { get; set; }

        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("frame_id")]
        public string FrameId { get; set; }

        [JsonPropertyName("sdk_version")]
        public string? SdkVersion { get; set; }
    }
    public class DiscordSDK
    {
        private static DiscordSDK? SingletonSDK { get; set; }

        private HttpClient client { get; set; }
        private NavigationManager navigation { get; set; }
        private IJSRuntime js { get; set; }
        private DiscordActivityBridge bridge { get; set; }
        private EventBus bus { get; set; }
        public string? clientId { get; private set; }
        bool registered {  get; set; }

        IDictionary<string, TaskCompletionSource<object>> pendingCommands = new Dictionary<string, TaskCompletionSource<object>>();
        public DiscordSDK(IJSRuntime js, HttpClient client, NavigationManager navigation, string clientId, EventBus bus)
        {
            this.client = client;
            this.js = js;
            this.navigation = navigation;
            this.bridge = new DiscordActivityBridge(js);
            this.clientId = clientId;
            this.bus = bus;
            SingletonSDK = this;
        }

        public async Task RegisterHandlers()
        {
            if(registered) return;
            await this.bus.on("message", OnMessage);
            await this.bus.on("READY", OnReady);

            registered = true;
        }
        public async Task OnMessage(JsonElement data)
        {
            Opcodes code = (Opcodes)int.Parse(data[0].ToString());
            //var taskSource = pendingCommands["nonce here"];
            Console.WriteLine("message");
        }

        public async Task OnReady(JsonElement data)
        {
            //var taskSource = pendingCommands["nonce here"];
            Console.WriteLine("ready");
        }
        [JSInvokable]
        public static void ReceiveDiscordMessage(JsonElement data)
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
        public async Task<object> SendCommandAsync(Opcodes opcode,object payload)
        {
            await RegisterHandlers();
            string nonce = Guid.NewGuid().ToString();

            // PostCommandAsync should return Task<T>
            var taskSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            await bridge.PostCommandAsync(opcode, payload, nonce);

            pendingCommands.Add(nonce, taskSource);
            return await taskSource.Task;
        }

        private string? GetQueryParam(string param)
        {
            var uri = new Uri(navigation.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return queryParams.TryGetValue(param, out var value) ? value.ToString() : null;
        }

        public async Task<object> Handshake()
        {
            string frameId = GetQueryParam("frame_id") ?? "";
            if(frameId == "")
                return Task.CompletedTask;
            HandshakePayload payload = new()
            {
                V = 1,
                Encoding = "json",
                ClientId = clientId!,
                FrameId = frameId,
                SdkVersion = "2.4.0"

            };

            return await SendCommandAsync(Opcodes.HANDSHAKE, payload);
        }
    }
}
