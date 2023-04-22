using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands;

public class RaidRoleService : ApplicationCommandModule
{
    private readonly IRaidRolesRepository _repo;
    private readonly IRaidRepository _raidRepo;
    private readonly IGuildSettingsRepository _guildRepo;
    private readonly IMessageBuilder _msg;
    private const string InitialResponse = "Thinking";
    private string? _title;
    private string? _description;
    private DiscordColor _color;

    public RaidRoleService(IRaidRolesRepository repository, IRaidRepository raidRepo,
        IGuildSettingsRepository guildRepo,
        IMessageBuilder message)
    {
        _repo = repository;
        _msg = message;
        _raidRepo = raidRepo;
        _guildRepo = guildRepo;
    }

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
            var getRaid = await _raidRepo.GetStatus(raidName); // gets the raid settings
            if (getRaid == null)
            {
                _title = "Error";
                _description = "Error getting raid";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            var guild = await _guildRepo.CheckGuildSettings(getRaid.GuildId); // gets the guild settings

            if (guild == null)
            {
                _title = "Error";
                _description = "Error getting the guild Id";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            var findGuild = await client.GetGuildAsync(getRaid.GuildId); // finds the correct guild to open the raid in

            if (findGuild == null)
            {
                _title = "Error";
                _description = "Error getting the guild";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, null, _color)));
                return;
            }

            if (guild.RaidChannelGroup == null)
            {
                _title = "Error";
                _description = "Channel Group does not exist";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            var findChannelGroup =
                await client.GetChannelAsync((ulong)guild
                    .RaidChannelGroup); // gets the channel group the channel will be created in

            if (getRaid.TierRole == null)
            {
                _title = "Error";
                _description = "Error retrieving roles";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            var getRoles =
                await _repo.GetRoles(getRaid.GuildId, getRaid.TierRole); // gets the roles associated with the tier

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

            if (getRaid.Info != null)
            {
                embed.AddField("Info", getRaid.Info, false);
            }
            else
            {
                embed.AddField("Info", "...", false);

            }
            embed.AddField("Date", $"<t:{convertTime}>");

            // Sends embed to the raid channel
            var msgEmbed = await channel.SendMessageAsync(embed);
            // adds reaction to raid channel
            var emoji = DiscordEmoji.FromGuildEmote(client, 987010296642142238);
            await msgEmbed.CreateReactionAsync(emoji);
            
            var createThread =
                await channelManager.CreateRaidChannelThread(channel, raidName + "-discussion",
                    ChannelType.PublicThread);

            var sendMessageInThread = createThread.SendMessageAsync("This is the thread to discuss the raid");

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