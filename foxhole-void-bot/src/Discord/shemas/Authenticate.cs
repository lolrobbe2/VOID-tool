using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace FoxholeBot.src.Discord.shemas
{
    public class AuthenticateResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("user")]
        public UserInfo User { get; set; } = new();

        [JsonPropertyName("scopes")]
        public List<string> Scopes { get; set; } = new();  // could be enum list or -1 as string

        [JsonPropertyName("expires")]
        public string Expires { get; set; } = string.Empty;

        [JsonPropertyName("application")]
        public ApplicationInfo Application { get; set; } = new();
    }

    public class UserInfo
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }  // optional / nullable

        [JsonPropertyName("public_flags")]
        public int PublicFlags { get; set; }

        [JsonPropertyName("global_name")]
        public string? GlobalName { get; set; }  // optional / nullable
    }

    public class ApplicationInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }  // optional / nullable

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("rpc_origins")]
        public List<string>? RpcOrigins { get; set; }  // optional

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
