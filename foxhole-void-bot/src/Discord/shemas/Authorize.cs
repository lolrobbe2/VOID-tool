using System.Text.Json.Serialization;
#nullable enable    
namespace FoxholeBot.src.Discord.shemas
{
    public class AuthorizeRequest
    {
        [JsonPropertyName("client_id")]
        public required string clientId { get; set; }
        [JsonPropertyName("scopes")]
        [JsonConverter(typeof(OAuthScopeArrayJsonConverter))]
        public required OAuthScope[] scopes { get; set; }
        [JsonPropertyName("response_type")]
        string? responseType = "code";
        [JsonPropertyName("response_type")]
        string? codeChallenge { get; set; }
        [JsonPropertyName("state")]
        string? state { get; set; }
        [JsonPropertyName("prompt")]
        string? prompt = "none";
        [JsonPropertyName("code_challenge_method")]
        string? ChallengeMethod = "S256";
    }

    public class AuthorizeResponse
    {
        [JsonPropertyName("code")]
        public required string code { get; set; }
    }
}
