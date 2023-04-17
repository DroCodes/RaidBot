using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands;

public class RaidManagementService : ApplicationCommandModule
{
    private readonly IRaidRepository? _repo;
    private readonly IMessageBuilder _messageBuilder;
    private const string? InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidManagementService(IRaidRepository repo, IMessageBuilder messageBuilder)
    {
        _repo = repo;
        _messageBuilder = messageBuilder;
    }
    
    [SlashCommand("createraid", "Create a raid")]
    public async Task CreateRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
    {
        var guildId = ctx.Guild.Id;
        string raidName = name.Replace(" ", "-").ToLower();
        // send the user a private message
        var dm = await ctx.Member.CreateDmChannelAsync();

        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)
                ));

        var saveRaid = await _repo?.CreateRaid(raidName, guildId)!;

        if (!saveRaid)
        {
            _title = "Error";
            _description = "There was a problem creating the raid.";
            _color = DiscordColor.Red;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
            return;
        }

        await dm.SendMessageAsync("Please continue with raid creation. \n" +
                                  "type /info /roles /tier /date /time");

        _title = "Success";
        _description = $"Raid creation started for {raidName}, continue in a private message with me";
        _color = DiscordColor.Green;
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
            ));
    }
    
    [SlashCommand("deleteraid", "Delete a raid")]
    public async Task DeleteRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
    {
        var guildId = ctx.Guild.Id;

        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)
                ));
            
        var deleteRaid = await _repo?.DeleteRaid(name, guildId)!;
            
        if (!deleteRaid)
        {
            _title = "Error";
            _description = $"Something went wrong deleting {name}";
            _color = DiscordColor.Red;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
            return;
        }

        _title = "Success";
        _description = $"{name} Successfully deleted";
        _color = DiscordColor.Green;
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
            ));
    }
}