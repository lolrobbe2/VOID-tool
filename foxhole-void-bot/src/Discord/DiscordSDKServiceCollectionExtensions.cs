using foxhole_void_bot.src.frontend.Pages;
using FoxholeBot.src.Discord;
using FoxholeBot.src.Discord.shemas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Security.Cryptography;

internal sealed class DiscordSDKOptions
{
    public static string ClientId { get; set; } = string.Empty;
    public static string ClientSecret { get; set; } = string.Empty;
    public static string JwtSecret { get; set; } = string.Empty;
    public static string algorithm { get; set; } = SecurityAlgorithms.HmacSha256;
}

public static class DiscordSDKServiceCollectionExtensions
{
    public static IServiceCollection AddDiscordSDK(this IServiceCollection services, string clientId, string clientSecret, string JwtSecret, string securityAlgorithm = SecurityAlgorithms.HmacSha256)
    {
        services.AddScoped<EventBus>();
        services.AddScoped<DiscordSDK>(sp =>
        {
            DiscordSDK sdk = new DiscordSDK(
                sp.GetRequiredService<IJSRuntime>(),
                sp.GetRequiredService<HttpClient>(),
                sp.GetRequiredService<NavigationManager>(), clientId, sp.GetRequiredService<EventBus>());
            return sdk;
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
            options.Cookie.Name = "DiscordSession";
            options.Cookie.HttpOnly = true; // Prevents JavaScript theft (XSS)
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Events = new CookieAuthenticationEvents
                {
                OnValidatePrincipal = async context =>
                {
                    string expiresAt = context.Principal.FindFirst("expires_at")?.Value;
                    if (DateTimeOffset.TryParse(expiresAt, out var expiry) && expiry < DateTimeOffset.UtcNow)
                    {
                        string refreshToken = context.Principal.FindFirst("refresh_token")?.Value;
                        //we should refresh the token
                    }
                }
            };
            });
        DiscordSDKOptions.ClientId = clientId;
        DiscordSDKOptions.ClientSecret = clientSecret;
        DiscordSDKOptions.JwtSecret = JwtSecret;
        DiscordSDKOptions.algorithm = securityAlgorithm;
        return services;
    }
}

