using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands
{
    public class GuildSettingsCommands : ApplicationCommandModule
    {
        private readonly IGuildSettingsRepository? _guildSettings;
        private readonly IMessageBuilder _messageBuilder;
        private const string _initialResponse = "Thinking";
        private string _title;
        private string _description;
        private DiscordColor _color;

        public GuildSettingsCommands(IGuildSettingsRepository guildSettingsRepository, IMessageBuilder MessageBuilder)
        {
            _guildSettings = guildSettingsRepository;
            _messageBuilder = MessageBuilder;
        }

        [SlashCommand("setguildid", "Set the guild id")]
        public async Task SetGuildIdCommand(InteractionContext ctx)
        {
            ulong guildId = ctx.Guild.Id;
            
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)));

            if (!await _guildSettings.AddGuildId(guildId))
            {
                _title = "Error";
                _description = "Guild Id has already been set";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description,
                        _color)));
            }
            else
            {
                _title = "Success";
                _description = $"Guild Id set to {guildId}";
                _color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
            }
        } // End SetGuildIdCommand

        [SlashCommand("setraidchannel", "Set the raid channel")]
        public async Task SetRaidChannelCommand(InteractionContext ctx,
            [Option("channel", "The channel to set")]
            DiscordChannel channel)
        {
            ulong guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)));

            if (channel.Type == ChannelType.Category)
            {
                _title = "Error";
                _description = "The channel is not a valid channel";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description,
                        _color)));
                return;
            }

            if (!await _guildSettings.SetRaidChannelId(guildId, channel.Id))
            {
                _title = "Error";
                _description =
                    "Something went wrong, please ensure the guildId has been set with the command /checksettings";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
            }
            else
            {
                _title = "Success";
                _description = $"Set raid channel to {channel}";
                _color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
            }
        } // End SetRaidChannelCommand

        [SlashCommand("setchannelgroup", "Sets the channel group for raids")]
        public async Task SetRaidChannelGroupCommand(InteractionContext ctx,
            [Option("category", "The category to create the channel in.")]
            DiscordChannel category)
        {
            var guildId = ctx.Guild.Id;
            ulong? channelCatId = category.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)));

            if (category.Type != ChannelType.Category)
            {
                _title = "Error";
                _description = $"This channel is not a category {category}";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description,
                        _color)));
                return;
            }

            if (!await _guildSettings.SetRaidChannelGroup(guildId, channelCatId))
            {
                _title = "Error";
                _description =
                    "Something went wrong, please make sure that the guild id has been set, if the problem persists contact an Admin. Thank you";
                _color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description,
                        _color)));
            }
            else
            {
                _title = "Success";
                _description = $"Category set to {category}";
                _color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(_title, _description, _color)));
            }
        } // end SetChannelGroupCommands

        [SlashCommand("checksettings", "Checks the guild settings")]
        public async Task checkguildsettingsCommands(InteractionContext ctx)
        {
            var guildId = ctx.Guild.Id;
            var guildChannels = ctx.Guild.Channels.ToList();

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)));

            var checkGuild = await _guildSettings.CheckGuildSettings(guildId);

            if (checkGuild == null)
            {
                _title = "Error";
                _description =
                    $"This guild doesn't exist in my DB, please manually set the guild Id by using the command /setguildid, thank you";
                _color = DiscordColor.Red;

                var errorTitle = "Error";

                ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)));
                return;
            }

            var guildChannelName = guildChannels.FirstOrDefault(x => x.Key == checkGuild[1]);
            var guildCategoryName = guildChannels.FirstOrDefault(x => x.Key == checkGuild[2]);

            _title = "Success";
            _description =
                $"Guild Id: {checkGuild[0]}\n" +
                $"Raid Channel Id: {guildChannelName.Value}\n" +
                $"Raid Channel Group: {guildCategoryName.Value}";
            _color = DiscordColor.Green;

            ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)));
        } // end CheckGuildSettingsCommand
    }
}