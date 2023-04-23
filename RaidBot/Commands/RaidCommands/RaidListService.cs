using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands;

public class RaidListService : ApplicationCommandModule
{
    private readonly IRaidRepository _repo;
    private readonly IMessageBuilder _msg;
    private const string? InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidListService(IRaidRepository repository, IMessageBuilder message)
    {
        _repo = repository;
        _msg = message;
    }
    
    [SlashCommand("raidlist", "Gets the list of active raids")]
    public async Task GetRaidListCommand(InteractionContext ctx)
    {
        var guildId = ctx.Guild.Id;

        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_msg.EmbedBuilder(InitialResponse)
                ));
        var getRaids = _repo.GetActiveRaids(guildId);

        if (getRaids == null)
        {
            _title = "Error";
            _description = "There was a problem getting the raid list";
            _color = DiscordColor.Red;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_msg.EmbedBuilder(_title, _description, _color)
                ));
            return;
        }

        _title = "Active Raids";
        _description = "";
        _color = DiscordColor.Green;

        foreach (var raidName in getRaids.Select(raid => raid.RaidName))
        {
            _description += $"{raidName}\n";
        }
            
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }
}