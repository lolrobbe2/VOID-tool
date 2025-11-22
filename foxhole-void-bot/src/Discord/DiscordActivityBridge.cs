namespace FoxholeBot.src.Discord
{
    using Microsoft.JSInterop;
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading.Tasks;
    public enum Opcodes
    {
        HANDSHAKE = 0,
        FRAME = 1,
        CLOSE = 2,
        HELLO = 3,
    }
    public enum Commands
    {
        AUTHORIZE = 0,
        GET_GUILDS = 1,
        GET_GUILD = 2,
        GET_CHANNEL = 3,
        GET_CHANNELS = 4,
        SELECT_VOICE_CHANNEL = 5,
        SELECT_TEXT_CHANNEL = 6,
        SUBSCRIBE = 7,
        UNSUBSCRIBE = 8,
        CAPTURE_SHORTCUT = 9,
        SET_CERTIFIED_DEVICES = 10,
        SET_ACTIVITY = 11,
        GET_SKUS = 12,
        GET_ENTITLEMENTS = 13,
        GET_SKUS_EMBEDDED = 14,
        GET_ENTITLEMENTS_EMBEDDED = 15,
        START_PURCHASE = 16,
        SET_CONFIG = 17,
        SEND_ANALYTICS_EVENT = 18,
        USER_SETTINGS_GET_LOCALE = 19,
        OPEN_EXTERNAL_LINK = 20,
        ENCOURAGE_HW_ACCELERATION = 21,
        CAPTURE_LOG = 22,
        SET_ORIENTATION_LOCK_STATE = 23,
        OPEN_INVITE_DIALOG = 24,
        GET_PLATFORM_BEHAVIORS = 25,
        GET_CHANNEL_PERMISSIONS = 26,

        AUTHENTICATE = 27,
        GET_ACTIVITY_INSTANCE_CONNECTED_PARTICIPANTS = 28,
        GET_QUEST_ENROLLMENT_STATUS = 29,
        GET_RELATIONSHIPS = 30,
        GET_USER = 31,
        INITIATE_IMAGE_UPLOAD = 32,
        INVITE_USER_EMBEDDED = 33,
        OPEN_SHARE_MOMENT_DIALOG = 34,
        QUEST_START_TIMER = 35,
        SHARE_INTERACTION = 36,
        SHARE_LINK = 37,
    }

    public class DiscordActivityBridge
    {
        private readonly IJSRuntime _js;

        public DiscordActivityBridge(IJSRuntime js)
        {
            _js = js;

        }
        public async Task RegisterDiscordMessageListenerAsync()
        {
            // Get the current assembly name dynamically
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

            string js = $@"
                window.addEventListener('message', function(event) {{
                    DotNet.invokeMethodAsync('{assemblyName}', 'ReceiveDiscordMessage', event.data);
                }});
            ";

            await _js.InvokeVoidAsync("eval", js);
        }
        public async Task PostCommandAsync(Opcodes opcode, object payload, string nonce, string sourceOrigin = "*")
        {
            string json = JsonSerializer.Serialize(payload);
            string js = $@"
                (function() {{
                    var payloadObj = JSON.parse({JsonSerializer.Serialize(json)});
                    payloadObj.nonce = {JsonSerializer.Serialize(nonce)};
                    var message = [{opcode}, payloadObj];

                    try {{ if (window.parent) window.parent.postMessage(message, '{sourceOrigin}'); }} catch(e){{}}
                    try {{ if (window.parent?.opener) window.parent.opener.postMessage(message, '{sourceOrigin}'); }} catch(e){{}}
                }})();
            ";
            await _js.InvokeVoidAsync("eval", js);
        }
    }

}
