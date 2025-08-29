using FoxholeBot.repositories;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

[SlashCommand("stockpiles", "Stockpiles command")]
public class StockpileCommands : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly StockpilesRepository _repository;

    public StockpileCommands(StockpilesRepository repository)
    {
        _repository = repository;
    }

    [SubSlashCommand("list", "Get stockpiles")]
    public async Task<string> ListStockpiles([SlashCommandParameter(Name = "region", Description = "the region/hex name", AutocompleteProviderType = typeof(RegionAutocompleteProvider))] string? region)
    {
        if (region == null)
            return await GetStockpilesRegion(region!);
        return await GetStockpiles();
    }

    private async Task<string> GetStockpiles()
    {
        StockPile[] stockpiles = await _repository.GetAllStockpilesAsync();

        if (stockpiles == null || stockpiles.Length == 0)
            return "No stockpiles found.";

        StringBuilder message = new StringBuilder();
        foreach (StockPile sp in stockpiles)
        {
            message.AppendLine($"**{sp.Name}**: `{sp.Code}` ({sp.Region}|{sp.Subregion})");
        }

        return message.ToString();
    }

    private async Task<string> GetStockpilesRegion(string region)
    {
        StockPile[] stockpiles = await _repository.GetRegionStockpiles(region);

        if (stockpiles == null || stockpiles.Length == 0)
            return "No stockpiles found.";

        StringBuilder message = new StringBuilder();
        foreach (StockPile sp in stockpiles)
        {
            message.AppendLine($"**{sp.Name}**: `{sp.Code}` ({sp.Region}|{sp.Subregion})");
        }

        return message.ToString();
    }
}

public class RegionAutocompleteProvider : IAutocompleteProvider<AutocompleteInteractionContext>
{
    private readonly StockpilesRepository _repository;

    public RegionAutocompleteProvider(StockpilesRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(ApplicationCommandInteractionDataOption option, AutocompleteInteractionContext context)
    {
        var userInput = option.Value?.ToString() ?? string.Empty;

        string[] regions = await _repository.GetStockpileRegions();

        return regions.Where(r => r.Contains(userInput, StringComparison.OrdinalIgnoreCase)).OrderBy(r => r).Take(5).Select(r => new ApplicationCommandOptionChoiceProperties(r, r));
    }
}

