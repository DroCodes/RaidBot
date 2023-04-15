using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands;

public class RaidStatusService : ApplicationCommandModule
{
    private readonly IRaidInfoRepository _repo;
    private readonly IRaidRepository _raidRepo;
    private readonly IMessageBuilder _msg;
    private const string InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidStatusService(IRaidInfoRepository repository, IRaidRepository raidRepo,
        IMessageBuilder message)
    {
        _repo = repository;
        _msg = message;
        _raidRepo = raidRepo;
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
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }

    [SlashCommand("Tier", "The tier of the raid")]
    public async Task TierCommand(InteractionContext ctx, [Option("RaidName", "The name of the raid")] string raidName,
        [Option("Tier", "The tier of the raid")]
        string tier)
    {
        string tierRole = tier.ToLower();
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        var setTier = await _repo.SaveRaidTier(raidName, tierRole);

        if (!setTier)
        {
            _title = "Error";
            _description = "There was an issue setting the tier, make sure the tier exists";
            _color = DiscordColor.Red;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        _title = "Success";
        _description = $"The tier for the raid was set to {tierRole}";
        _color = DiscordColor.Green;

        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    } // end tier command
 
    // command to save the raid date to the database
    [SlashCommand("date", "Set the date for the raid")]
    public async Task SetDateCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName,
        [Option("Date", "The date mm-dd-yyyy")]
        string date)

    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));
        var formatDate = new FormatDateTime();
        try
        {
            DateTime parseDate = formatDate.ParseDateTime(date);
            var saveDate = await _repo.SaveDateTime(raidName, parseDate);

            if (!saveDate)
            {
                _title = "Error";
                _description = "There was an issue setting the Date, make sure you formatted the date properly";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            string convert = formatDate.ConvertToEasternTime(parseDate);
            long convertToUnix = formatDate.ParseUnixTime(convert);

            _title = "Success";
            _description = $"The Date for {raidName} was set to <t:{convertToUnix}>";
            _color = DiscordColor.Green;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    } // end Date command
    
    [SlashCommand("Status", "Gets the status of a raid")]
    public async Task GetStatusCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        var status = await _raidRepo.GetStatus(raidName);
        if (status == null)
        {
            _title = "Error";
            _description = "There was an issue getting the status";
            _color = DiscordColor.Red;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        try
        {
            var format = new FormatDateTime();
            string convertTime = format.ConvertToEasternTime(((DateTime)status.Date));
            // Console.WriteLine(status.Roles.HealerRole);


            var embed = new DiscordEmbedBuilder()
            {
                Title = status.RaidName,
            };

            status.Info ??= "Empty";
            status.TierRole ??= "0";

            if (convertTime != "Saturday, January 01, 2000 1:00 AM")
            {
                long convertToUnix = format.ParseUnixTime(convertTime);
                embed.AddField("Raid Date:", $"<t:{convertToUnix}>", inline: false);
            }
            else
            {
                embed.AddField("Raid Date:", "Date not set", inline: false);
            }

            embed.AddField("Raid Info:", status.Info, inline: false);
            embed.AddField("Raid Tier:", $"Tier: {status.TierRole}", inline: false);
            if (status.Roles != null)
            {
                embed.AddField("Raid Roles:", $"Tanks: {status.Roles.TankRole}" +
                                              $"\nHealers: {status.Roles.HealerRole}" +
                                              $"\nDps: {status.Roles.HealerRole}", inline: false);
            }
            else
            {
                embed.AddField("Raid Roles:", $"Tanks: Not set" +
                                              $"\nHealers: Not set" +
                                              $"\nDps: Not set", inline: false);
            }

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } // end status command
}