
using FoxholeBot;
using FoxholeBot.repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddScoped<StockpilesRepository>()
    .AddScoped<StockPileItemsRepository>()
    .AddScoped<FoxholeRepository>()
    .AddFirebase()
    .AddDiscordGateway((options, _) =>
    {
        options.Token = Config.GetBotToken();
    })
    .AddApplicationCommands((options) =>
    {
        options.AutoRegisterCommands = true;
    })
    .AddRazorPages()
    .AddRazorRuntimeCompilation().WithRazorPagesRoot("/src/frontend/Pages");
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();

    options.ViewLocationFormats.Add("/src/frontend/Components/{1}/{0}.cshtml");
});
builder.Services.Configure<AssetOptions>(options =>
{
    options.AssetMode = Config.GetAssetMode();
});
builder.Services.AddMvc();







builder.Services.AddControllers();
builder.Services.AddOpenApi();
var host = builder.Build();

if (host.Environment.IsDevelopment())
{
    host.MapOpenApi();
    host.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "VOID api"));
}
host.MapControllers();
host.MapRazorPages();
// Add commands from modules
host.AddApplicationCommandModule(typeof(StockpileCommands));
await host.RunAsync();
