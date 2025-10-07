using FoxholeBot.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCord.Rest;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoxholeBot.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        DiscordRepository _discordRepo { get; set; }

        public HealthController(DiscordRepository discord) {
           _discordRepo = discord;
        }
        [HttpHead]
        public IActionResult Get() => Ok(new { status = "ok" });

        [HttpPost("restart")]
        public async Task<IActionResult> PostAsync() {
            Guild guild = await _discordRepo.GetGuild("The VOID");
            GuildChannel channel = await _discordRepo.GetChannel(guild, 1300565932065685524);
            await _discordRepo.SendMessage(channel, "VOID-tool attempting restart...");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.GetDeployUri());
                response.EnsureSuccessStatusCode(); // optional: throws if the status code is not 2xx
            }
            await _discordRepo.SendMessage(channel, "VOID-tool restarting");
            return Ok("restart");
        }
    }

}