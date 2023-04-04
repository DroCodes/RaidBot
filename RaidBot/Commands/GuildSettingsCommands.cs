using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands
{
    public class GuildSettingsCommands : ApplicationCommandModule
    {
        private static IGuildSettingsRepository? _guildSettings;
        private readonly IMessageBuilder _messageBuilder;
        public string InitialResponse = "Thinking";
        public string Title;
        public string Description;
        public DiscordColor Color;

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
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)));

            if (!await _guildSettings.AddGuildId(guildId))
            {
                Title = "Error";
                Description = "Guild Id has already been set";
                Color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description,
                        Color)));
            }
            else
            {
                Title = "Success";
                Description = $"Guild Id set to {guildId}";
                Color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(Title, Description, Color)
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
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)));

            if (channel.Type == ChannelType.Category)
            {
                Title = "Error";
                Description = "The channel is not a valid channel";
                Color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description,
                        Color)));
                return;
            }

            if (!await _guildSettings.SetRaidChannelId(guildId, channel.Id))
            {
                Title = "Error";
                Description =
                    "Something went wrong, please ensure the guildId has been set with the command /checksettings";
                Color = DiscordColor.Red;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(Title, Description, Color)
                ));
            }
            else
            {
                Title = "Success";
                Description = $"Set raid channel to {channel}";
                Color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(Title, Description, Color)
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
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)));

            if (category.Type != ChannelType.Category)
            {
                Title = "Error";
                Description = $"This channel is not a category {category}";
                Color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description,
                        Color)));
                return;
            }

            if (!await _guildSettings.SetRaidChannelGroup(guildId, channelCatId))
            {
                Title = "Error";
                Description =
                    "Something went wrong, please make sure that the guild id has been set, if the problem persists contact an Admin. Thank you";
                Color = DiscordColor.Red;

                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description,
                        Color)));
            }
            else
            {
                Title = "Success";
                Description = $"Category set to {category}";
                Color = DiscordColor.Green;

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(
                    _messageBuilder.EmbedBuilder(Title, Description, Color)));
            }
        } // end SetChannelGroupCommands

        [SlashCommand("checksettings", "Checks the guild settings")]
        public async Task checkguildsettingsCommands(InteractionContext ctx)
        {
            var guildId = ctx.Guild.Id;
            var guildChannels = ctx.Guild.Channels.ToList();

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)));

            var checkGuild = await _guildSettings.CheckGuildSettings(guildId);

            if (checkGuild == null)
            {
                Title = "Error";
                Description =
                    $"This guild doesn't exist in my DB, please manually set the guild Id by using the command /setguildid, thank you";
                Color = DiscordColor.Red;

                var errorTitle = "Error";

                ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description, Color)));
                return;
            }

            var guildChannelName = guildChannels.FirstOrDefault(x => x.Key == checkGuild[1]);
            var guildCategoryName = guildChannels.FirstOrDefault(x => x.Key == checkGuild[2]);

            Title = "Success";
            Description =
                $"Guild Id: {checkGuild[0]}\n" +
                $"Raid Channel Id: {guildChannelName.Value}\n" +
                $"Raid Channel Group: {guildCategoryName.Value}";
            Color = DiscordColor.Green;

            ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder(Title, Description, Color)));
        } // end CheckGuildSettingsCommand
    }
}