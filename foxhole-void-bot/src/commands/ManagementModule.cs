using NetCord.Services.ApplicationCommands;
using System.Net.Http;
using System.Threading.Tasks;
#nullable enable
namespace FoxholeBot.commands
{
    [SlashCommand("management", "management commands")]
    public class ManagementCommands : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("restart", "Get stockpiles")]
        public async Task<string> RestartAsync([SlashCommandParameter(Name = "password", Description = "the management password")] string? password = null)
        {
            if (password != Config.GetPassWord())
            {
                return "unautherized";
            }
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.GetDeployUri());
                response.EnsureSuccessStatusCode(); // optional: throws if the status code is not 2xx
            }
            return "VOID-tool restarting";
        }
    }
}
