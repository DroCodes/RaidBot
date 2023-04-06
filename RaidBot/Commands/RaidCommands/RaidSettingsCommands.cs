using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands;

public class RaidSettingsCommands : ApplicationCommandModule
{
    private readonly IRaidSettingsRepository _repo;
    private readonly IMessageBuilder _msg;
    private const string InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidSettingsCommands(IRaidSettingsRepository repository, IMessageBuilder message)
    {
        _repo = repository;
        _msg = message;
    }

    [SlashCommand("info", "set the info for the raid")]
    [RequireDirectMessage]
    public async Task InfoCommand(InteractionContext ctx,
        [Option("raidname", "The name of the raid")]
        string raidName, [Option("Info", "Info about the raid")] string info)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        var addInfo = await _repo.SaveRaidInfo(raidName.ToLower(), info);

        if (!addInfo)
        {
            _title = "Error";
            _description = "there was a problem saving the info";
            _color = DiscordColor.Red;
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        _title = "Success";
        _description = $"The raid info was set to {info}";
        _color = DiscordColor.Green;
        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));    }
}