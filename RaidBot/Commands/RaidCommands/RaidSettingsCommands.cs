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

            ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        _title = "Success";
        _description = $"The tier for the raid was set to {tierRole}";
        _color = DiscordColor.Green;

        ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }

    // command to save the raid date to the database
    [SlashCommand("date", "Set the date for the raid")]
    public async Task SetDateCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName,
        [Option("Date", "The date mm-dd-yyyy")]
        string date)

    {
        ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
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

                ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            string convert = formatDate.ConvertToEasternTime(parseDate);

            _title = "Success";
            _description = $"The Date for {raidName} was set to {convert}";
            _color = DiscordColor.Green;

            ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [SlashCommand("Status", "Gets the status of a raid")]
    public async Task GetStatusCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName)
    {
        ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        var status = await _repo.GetStatus(raidName);
        if (status == null)
        {
            _title = "Error";
            _description = "There was an issue getting the status";
            _color = DiscordColor.Red;

            ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        try
        {
            var format = new FormatDateTime();
            string convertTime = format.ConvertToEasternTime((DateTime)status.Date);

            var embed = new DiscordEmbedBuilder()
            {
                Title = status.RaidName,
            };

            if (status.Info == null)
            {
                status.Info = "Empty";
            }

            if (status.TierRole == null)
            {
                status.TierRole = "0";
            }

            embed.AddField("Raid Date:", convertTime, inline: false);
            embed.AddField("Raid Info:", status.Info, inline: false);
            embed.AddField("Tier:", $"Tier: {status.TierRole}", inline: true);
            if (status.Roles != null)
            {
                embed.AddField("Roles:", $"Tanks: /n{status.Roles.TankRole}" +
                                         $"/nHealers: {status.Roles.HealerRole}" +
                                         $"/nDps: {status.Roles.HealerRole}", inline: true);
            }
            else
            {
                embed.AddField("Roles:", $"Tanks: Not set" +
                                         $"\nHealers: Not set" +
                                         $"\nDps: Not set", inline: false);
            }

            ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } // end status command

    [SlashCommand("roles", "Adds roles to raid")]
    public async Task AddRolesCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")] string raidName,
        [Option("Tank", "Enter the number of tanks")] double tank,
        [Option("Healer", "Enter the number of healers")]
        double healer,
        [Option("Dps", "Enter the number of Dps")]
        double dps)
    {
        ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        double[] roles = new[] { tank, healer, dps };
        int[] intRoles = roles.Select(Convert.ToInt32).ToArray();

        var saveRoles = await _repo.SetRolesForRaid(raidName, intRoles);

        if (!saveRoles)
        {
            _title = "Error";
            _description = "There was an issue saving the roles";
            _color = DiscordColor.Red;

            ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        _title = "Success";
        _description = $"{raidName} was set with {tank} tanks, {healer}, healers, {dps}, dps";
        _color = DiscordColor.Green;

        ctx.EditResponseAsync(
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }
}