using FoxholeBot.src.Discord;
using FoxholeBot.src.Discord.shemas;
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
            services.AddScoped<EventBus>();
            services.AddScoped<DiscordSDK>(sp =>
            {
                var sdk = new DiscordSDK(
                    sp.GetRequiredService<IJSRuntime>(),
                    sp.GetRequiredService<HttpClient>(),
                    sp.GetRequiredService<NavigationManager>(),clientId, sp.GetRequiredService<EventBus>());
                return sdk;
            });

            return services;
        }
    }

