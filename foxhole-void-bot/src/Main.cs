
using FoxholeBot;
using FoxholeBot.repositories;
using Microsoft.AspNetCore.Builder;
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
    .AddFirebase()
    .AddDiscordGateway((options, _) =>
    {
        options.Token = Config.GetBotToken();
    })
    .AddApplicationCommands();



builder.Services.AddControllers();
builder.Services.AddOpenApi();
var host = builder.Build();

if (host.Environment.IsDevelopment())
{
    host.MapOpenApi();
    host.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "VOID api"));
}
host.MapControllers();
// Add commands from modules
host.AddModules(typeof(Program).Assembly);

await host.RunAsync();
