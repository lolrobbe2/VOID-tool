using FoxholeBot.repositories;
using NetCord.Services.ApplicationCommands;
using System.Net.Http;
using System.Threading.Tasks;
#nullable enable
namespace FoxholeBot.commands
{
    [SlashCommand("management", "management commands")]
    public class ManagementCommands : ApplicationCommandModule<ApplicationCommandContext>
    {
        private readonly StockPileItemsRepository _itemsRepository;
        private readonly StockpilesRepository _repository;
        public ManagementCommands(StockPileItemsRepository itemsRepository, StockpilesRepository repository) 
        {
            _itemsRepository = itemsRepository;
            _repository = repository;
        }

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

        [SubSlashCommand("reset", "resets inteternal storage")]
        public async Task<string> ResetAsync([SlashCommandParameter(Name = "password", Description = "the management password")] string? password = null)
        {
            if (password != Config.GetPassWord())
            {
                return "unautherized";
            }
            StockPile[] stockpiles = await _repository.GetAllStockpilesAsync();
            foreach (StockPile stockPile in stockpiles)
            {
                await _itemsRepository.ClearStockpileItems(stockPile);
            }

            await _repository.ClearStockpiles();

            return "Succesfuly reset VOID-tool";
        }
    }
}
