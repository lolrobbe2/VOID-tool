using DocumentFormat.OpenXml.Wordprocessing;
using FoxholeBot.src.Discord.shemas;
using FoxholeBot.src.Discord.shemas.commands;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
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
        public string SdkVersion { get; set; }
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
        public string? frameId { get; private set; }
        public string? instanceId { get; private set; }
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
            await bus.on("message",OnMessage);
            await bus.on("READY", OnReady);

            registered = true;
        }

        public async Task OnMessage(object data)
        {
            //var taskSource = pendingCommands["nonce here"];
            Console.WriteLine("message");
        }
        public async Task OnReady(object data)
        {
            //var taskSource = pendingCommands["nonce here"];
            Console.WriteLine("ready");
        }
        [JSInvokable]
        public static void ReceiveDiscordMessage(object data)
        {
            // data is the message received via postMessage
            Console.WriteLine("Discord message received:");
        }

        public async Task<TResult> SendCommand<TCommand, TArgs, TResult>(ICommandStandard<TCommand, TArgs, TResult> command)
          where TCommand : Enum
          where TResult : class
        {
            return await SendCommand<TCommand, TArgs, TResult>(command.request);
        }
        public async Task<TResult> SendCommand<TCommand, TArgs, TResult>(SendCommandPayload<TCommand, TArgs> payload)
           where TCommand : Enum
           where TResult : class
        {
            return (TResult) await SendCommandAsync(payload);
        }

        /// <summary>
        /// this function sends a commands to the bridge and then ackowladges it
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<object> SendCommandAsync(object payload)
        {
            await RegisterHandlers();
            string nonce = Guid.NewGuid().ToString();

            // PostCommandAsync should return Task<T>
            var taskSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            await bridge.PostCommandAsync(Opcodes.FRAME,payload, nonce);

            pendingCommands.Add(nonce, taskSource);
            return await taskSource.Task;
        }

        private string? GetQueryParam(string param)
        {
            var uri = new Uri(navigation.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return queryParams.TryGetValue(param, out var value) ? value.ToString() : null;
        }

        public async Task Handshake()
        {
            frameId = GetQueryParam("frame_id") ?? "";
            if (frameId == "")
                return;

            HandshakePayload payload = new HandshakePayload()
            {
                ClientId = clientId!,
                V = 1,
                Encoding = "json",
                FrameId = frameId,
                SdkVersion = "2.4.0"
            };
            await bridge.PostCommandDirectAsync(Opcodes.HANDSHAKE,payload, "https://localhost:53409/");
        }

        public void HandleHandShake()
        {
            Console.WriteLine("Handshake");
        }

        public void HandleFrame(JsonElement frameData)
        {
            pendingCommands[frameData.GetProperty("nonce").ToString()].SetResult(frameData[0]);
            pendingCommands.Remove(frameData.GetProperty("nonce").ToString());  
        }
    }
}
