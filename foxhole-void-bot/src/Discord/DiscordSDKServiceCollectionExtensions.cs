using FoxholeBot.src.Discord;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Net.Http;

    public sealed class DiscordSDKOptions
    {
        public string ClientId { get; set; } = string.Empty;
    }

    public static class DiscordSDKServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordSDK(this IServiceCollection services, string clientId)
        {
            services.AddScoped<DiscordSDK>(sp =>
            {
                var sdk = new DiscordSDK(
                    sp.GetRequiredService<IJSRuntime>(),
                    sp.GetRequiredService<HttpClient>(),
                    sp.GetRequiredService<NavigationManager>(),clientId);
                return sdk;
            });

            return services;
        }
    }

