using System.Text;
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
    private readonly IGuildSettingsRepository _guildRepo;
    private readonly IMessageBuilder _msg;
    private const string InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidSettingsCommands(IRaidSettingsRepository repository, IGuildSettingsRepository guildRepo,
        IMessageBuilder message)
    {
        _repo = repository;
        _msg = message;
        _guildRepo = guildRepo;
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
    }

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
            throw;
        }
    }

    [SlashCommand("Status", "Gets the status of a raid")]
    public async Task GetStatusCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        var status = await _repo.GetStatus(raidName);
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

    [SlashCommand("roles", "Adds roles to raid")]
    public async Task AddRolesCommand(InteractionContext ctx,
        [Option("RaidName", "The name of the raid")]
        string raidName,
        [Option("Tank", "Enter the number of tanks")]
        double tank,
        [Option("Healer", "Enter the number of healers")]
        double healer,
        [Option("Dps", "Enter the number of Dps")]
        double dps)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        double[] roles = new[] { tank, healer, dps };
        int[] intRoles = roles.Select(Convert.ToInt32).ToArray();

        var saveRoles = await _repo.SetRolesForRaid(raidName, intRoles);

        if (!saveRoles)
        {
            _title = "Error";
            _description = "There was an issue saving the roles";
            _color = DiscordColor.Red;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        _title = "Success";
        _description = $"{raidName} was set with {tank} tanks, {healer}, healers, {dps}, dps";
        _color = DiscordColor.Green;

        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }

    [SlashCommand("Open", "Opens the raid")]
    public async Task OpenRaidCommand(InteractionContext ctx,
        [Option("RaidName", "The name of raid to open")]
        string raidName)
    {
        var client = ctx.Client;
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        try
        {
            var getRaid = await _repo.GetStatus(raidName); // gets the raid settings
            if (getRaid == null)
            {
                _title = "Error";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            var guild = await _guildRepo.CheckGuildSettings(getRaid.GuildId); // gets the guild settings

            if (guild == null)
            {
                _title = "Error";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            var findGuild = await client.GetGuildAsync(getRaid.GuildId); // finds the correct guild to open the raid in


            if (findGuild == null)
            {
                _title = "Error";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            if (guild.RaidChannelGroup == null)
            {
                _title = "Error";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            var findChannelGroup = await client.GetChannelAsync((ulong)guild.RaidChannelGroup); // gets the channel group the channel will be created in

            if (getRaid.TierRole == null)
            {
                _title = "Error";
                _description = "Error retrieving roles";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }
            var getRoles = await _repo.GetRoles(getRaid.GuildId, getRaid.TierRole); // gets the roles associated with the tier

            var channelManager = new ChannelManager(); // new instance of ChannelManager class
            // Method to create a new channel
            var newChannel =
                await channelManager.CreateRaidChannelAsync(findGuild, getRaid.RaidName, ChannelType.Text,
                    findChannelGroup);

            if (newChannel == null)
            {
                _title = "Error";
                _description = "Error creating the channel, is this raid already open?";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }
            
            // gets the newly created channel
            var channel = await client.GetChannelAsync(newChannel.Id);
            var roles = findGuild.Roles;

            var message = ""; // message that contains role mentions

            // Loops to find the roles
            foreach (var r in roles)
            {
                foreach (var role in getRoles)
                {
                    if (role.RoleName == r.Value.Name)
                    {
                        message += r.Value.Mention;
                        Console.WriteLine("Roles " + message);
                    }
                }
            }
            
            var msg = await channel.SendMessageAsync(message); // msg with role mentions
            
            // builds embed
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Raid Info"
            };

            var formatTime = new FormatDateTime();
            var convertTime = formatTime.DateTimeToUnixTime((DateTime)getRaid.Date);

            embed.AddField("Info", getRaid.Info, false);
            embed.AddField("Date", $"<t:{convertTime}>");

            // Sends embed to the raid channel
            var msgEmbed = await channel.SendMessageAsync(embed);
            // adds reaction to raid channel
            var emoji = DiscordEmoji.FromGuildEmote(client, 987010296642142238);
            await msgEmbed.CreateReactionAsync(emoji);

            _title = "Success";
            _description = "Raid has been opened and channel has been created";
            _color = DiscordColor.Green;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}