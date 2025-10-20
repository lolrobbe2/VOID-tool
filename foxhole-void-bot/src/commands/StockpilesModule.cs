using FoxholeBot.modal;
using FoxholeBot.repositories;
using FoxholeBot.types;
using Microsoft.VisualBasic;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;

#nullable enable

[SlashCommand("stockpiles", "Stockpiles command")]
public class StockpileCommands : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly StockPileItemsRepository _itemsRepository;
    private readonly StockpilesRepository _repository;
    private readonly FoxholeRepository _foxhole;
    private readonly DiscordRepository _discord;


    public StockpileCommands(StockPileItemsRepository itemsRepository, StockpilesRepository repository, FoxholeRepository foxhole, DiscordRepository discordRepository)
    {
        _itemsRepository = itemsRepository;
        _repository = repository;
        _foxhole = foxhole;
        _discord = discordRepository;
    }

    [SubSlashCommand("list", "Get stockpiles")]
    public async Task<string> ListStockpiles([SlashCommandParameter(Name = "region", Description = "the region/hex name", AutocompleteProviderType = typeof(RegionAutocompleteProvider))] string? region)
    {
        if (Context.User is GuildUser member && !await _discord.UserHasRole(member, "FH-VOID-Regiment"))
        {
            return "unautherized";
        }
            if (string.IsNullOrEmpty(region) || region.Equals("none"))
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

    [SubSlashCommand("create", "Update Stockpiles")]
    public async Task CreateStockpile([Description("Select the region of the stockpile"), SlashCommandParameter(AutocompleteProviderType = typeof(AvailableRegionAutocompleteProvider))] string region, [Description("Subregion of the stockpile")] string subregion, [Description("the name of the stockpile")] string name, [Description("6 digit code of the stockpile")] string code)
    {
        if (Context.User is GuildUser member && await _discord.UserHasRole(member, "FH-VOID-Regiment"))
        {
            await _repository.CreateStockPile(name, region, subregion, code);
        }
    }

    [SubSlashCommand("update", "Update Stockpiles")]
    public async Task UpdateStockpile([Description("TSV file containing stockpile data")] Attachment attachment)
    {
        if (Context.User is GuildUser member && !await _discord.UserHasRole(member, "FH-VOID-Regiment"))
        {
            return;
        }

        if (!attachment.FileName.EndsWith(".tsv", StringComparison.OrdinalIgnoreCase))

            return;

        await this.Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        using var httpClient = new HttpClient();
        string content = await httpClient.GetStringAsync(attachment.Url);

        InvetoryReport report = new InvetoryReport(content);

        // TODO: Parse and process the TSV content

        foreach (var (stockpileName, items) in report.SplitByStockpile())
        {
            StockPile? stockpile = await _repository.GetStockPileAsync(stockpileName);
            if (stockpile is null)
            {
                await Context.Interaction.SendFollowupMessageAsync($"Stockpile with name: {stockpileName}");
            }
            else
            {

                var stockpileItems = items.Select(item => new StockPileItem() { Name = item.Name, Count = item.Amount, Crated = item.Crated.Equals("true") });
                await _itemsRepository.SetStockPileItemsAsync(stockpile, stockpileItems.ToArray());
                await Context.Interaction.SendFollowupMessageAsync(
                new InteractionMessageProperties
                {
                    Embeds = [
                        new EmbedProperties
                        {
                            Author = new EmbedAuthorProperties
                            {
                                 Name = Context.User.Username,   // the user's name
                                    IconUrl = Context.User.DefaultAvatarUrl?.ToString() // optional: show avatar
                            },
                            Title= $"Updated Stockpile",
                            Fields =
                            [
                                new EmbedFieldProperties(){
                                    Name = "Name",
                                    Value = stockpile.Name,
                                    Inline = true
                                },
                                new EmbedFieldProperties(){
                                    Name = "Region",
                                    Value = stockpile.Region,
                                    Inline = true
                                },
                                new EmbedFieldProperties(){
                                    Name = "SubRegion",
                                    Value = stockpile.Subregion,
                                    Inline = true
                                },
                            ],
                            Timestamp = DateTime.Now,



                        }
                    ]
                });
            }
        }
        return;
    }

    [SubSlashCommand("report", "create XLSX file of selected stockpile")]
    public async Task ReportStockpile([Description("Stockpile name")] string name)
    {
        if (Context.User is GuildUser member && !await _discord.UserHasRole(member, "FH-VOID-Regiment"))
        {
            await Context.Channel.SendMessageAsync("unautherized");
            return;
        }

        StockPile? stockpile = await _repository.GetStockPileAsync(name);
        if (stockpile is null)
        {
            await Context.Channel.SendMessageAsync("Stockpile does not exist!");
            return;
        }

        OutputFormatter outputFormatter = new OutputFormatter();
        outputFormatter.AddStockpile(stockpile, await _itemsRepository.GetStockPileItemsAsync(stockpile));

        await Context.Interaction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties()
        {
            Attachments = [new AttachmentProperties($"{stockpile.Region}_{stockpile.Name}.xlsx",outputFormatter.Build())]
        }));

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
        regions = regions.Append("none").ToArray();
        return regions.Where(r => r.Contains(userInput, StringComparison.OrdinalIgnoreCase)).OrderBy(r => r).Take(5).Select(r => new ApplicationCommandOptionChoiceProperties(r, r));
    }
}


public class AvailableRegionAutocompleteProvider : IAutocompleteProvider<AutocompleteInteractionContext>
{
    private readonly FoxholeRepository _repository;

    public AvailableRegionAutocompleteProvider(FoxholeRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(ApplicationCommandInteractionDataOption option, AutocompleteInteractionContext context)
    {
        var userInput = option.Value?.ToString() ?? string.Empty;

        string[] regions = _repository.GetAllRegions();
        regions = regions.Append("none").ToArray();
        return regions.Where(r => r.Contains(userInput, StringComparison.OrdinalIgnoreCase)).OrderBy(r => r).Take(5).Select(r => new ApplicationCommandOptionChoiceProperties(r, r));
    }
}



