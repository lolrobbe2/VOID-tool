using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
#nullable enable
namespace FoxholeBot.Services
{
    public static class DiscordAuthenticationExtensions
    {
        private static bool registered = false;
        public static IServiceCollection AddDiscordAuthentication(
            this IServiceCollection services,
            string clientId,
            string clientSecret,
            string redirectUri = "127.0.0.1"
        )
        {
            if (registered)
            {
                return services;
            }
            services.AddRouting();
            services.AddSingleton<IStartupFilter>(new DiscordOAuthStartupFilter(clientId, redirectUri));

            return services;
        }

        private class DiscordOAuthStartupFilter : IStartupFilter
        {
            private readonly string clientId;
            private readonly string redirectUri;

            public DiscordOAuthStartupFilter(string clientId, string redirectUri)
            {
                this.clientId = clientId;
                this.redirectUri = redirectUri;
            }

            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                return app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/api/autherize/", async context =>
                        {
                            string url =
                                $"https://discord.com/oauth2/authorize" +
                                $"?client_id={Uri.EscapeDataString(clientId)}" +
                                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                                $"&response_type=code" +
                                $"&scope=identify";

                            context.Response.Redirect(url);
                            await Task.CompletedTask;
                        });
                    });

                    next(app);
                };
            }
        }

    }
}