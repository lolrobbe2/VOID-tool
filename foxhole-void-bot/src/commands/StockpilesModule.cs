using FoxholeBot.repositories;
using NetCord.Services.ApplicationCommands;
using System.Text;
using System.Threading.Tasks;

[SlashCommand("stockpiles", "Stockpiles command")]
public class StockpileCommands : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly StockpilesRepository _repository;

    public StockpileCommands(StockpilesRepository repository)
    {
        _repository = repository;
    }

    [SubSlashCommand("list", "Get stockpiles")]
    public async Task<string> ListStockpiles()
    {
        var stockpiles = await _repository.GetAllStockpilesAsync() ;

        if (stockpiles == null || stockpiles.Length == 0)
            return "No stockpiles found.";

        var message = new StringBuilder();
        foreach (var sp in stockpiles)
        {
            message.AppendLine($"**{sp.Name}** — `{sp.Code}` ({sp.Region}/{sp.Subregion})");
        }

        return message.ToString();
    }

}
