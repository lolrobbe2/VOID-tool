
using foxhole_void_bot.src.frontend.Pages;
using FoxholeBot;
using FoxholeBot.commands;
using FoxholeBot.Discord;
using FoxholeBot.modal;
using FoxholeBot.repositories;
using FoxholeBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetCord;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Rest;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddMemoryCache()
    .AddScoped<StockpilesRepository>()
    .AddScoped<StockPileItemsRepository>()
    .AddScoped<FoxholeRepository>()
    .AddScoped<DiscordRepository>()
    .AddFirebase()
    .AddDiscordGateway((options, _) =>
    {
        options.Token = Config.GetBotToken();
    })
    .AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>()
    .AddApplicationCommands((options) =>
    {
        options.AutoRegisterCommands = true;
    })


    .AddRazorPages()
    .AddRazorRuntimeCompilation().WithRazorPagesRoot("/src/frontend/Pages");
builder.Services.Configure<AssetOptions>(options =>
{
    options.AssetMode = Config.GetAssetMode();
});
builder.Services.AddScoped(sp =>
{
    NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
}).AddDiscordAuthentication(Config.GetBotClientId(), Config.GetBotClientSecret())
;





builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddServerSideBlazor();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<DiscordAuthorizeFilter>();
});
var host = builder.Build();

if (host.Environment.IsDevelopment())
{
    host.MapOpenApi();
    host.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "VOID api");
        
        }
    );
}
host.MapControllers();
host.MapRazorPages();
host.UseRouting();
host.MapBlazorHub();
host.MapFallbackToPage("/_Host");
if (host.Environment.IsDevelopment())
{
    host.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "obj", "Debug", "net9.0", "win-x64", "scopedcss", "bundle")),
        RequestPath = "/css"
    });
} else
{
    host.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
          Path.Combine(host.Environment.ContentRootPath, "wwwroot")),
        RequestPath = "/css"
    });
}

// Add commands from modules
host.AddApplicationCommandModule(typeof(StockpileCommands));
host.AddApplicationCommandModule(typeof(ManagementCommands));
host.AddComponentInteractionModule(typeof(StockpileModule));
host.AddEntryPointCommand("entrypoint", "Entry Point Command");
await host.RunAsync();
