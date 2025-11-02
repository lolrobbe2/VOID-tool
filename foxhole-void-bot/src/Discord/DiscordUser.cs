using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable

namespace FoxholeBot.src.Discord
{
    public class DiscordUser
    {
        private HttpClient client { get; set; }
        public DiscordUser(HttpClient client)
        {
            this.client = client;
        }
        public async Task<string?> GetAuthorizationCodeAsync()
        {
            // Step 1: Initiate the OAuth flow
            var response = await client.GetAsync("/api/authorize");

            // Step 2: Follow redirect manually (Discord will redirect with ?code=...)
            if (response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.RedirectMethod)
            {
                var redirectUrl = response.Headers.Location?.ToString();
                if (redirectUrl != null)
                {
                    var uri = new Uri(redirectUrl);
                    var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    return queryParams["code"];
                }
            }

            return null;
        }
    }
}
