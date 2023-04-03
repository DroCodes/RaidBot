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
        public GuildSettingsCommands(IGuildSettingsRepository guildSettingsRepository, IMessageBuilder MessageBuilder)
        {
            _guildSettings = guildSettingsRepository;
            _messageBuilder = MessageBuilder;
        }

        [SlashCommand("setguildid", "Set the guild id")]
        public  async Task SetGuildIdCommand(InteractionContext ctx)
        {
            ulong guildId = ctx.Guild.Id;
            _messageBuilder.Title = "Thinking";
            
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder()));

                if (!await _guildSettings.AddGuildId(guildId))
                {
                    _messageBuilder.Title = "Guild Id has already been set";
                    _messageBuilder.Color= DiscordColor.Red;
                    
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder()));                
                }
                else
                {
                    _messageBuilder.Title = "Success";
                    _messageBuilder.Description = $"Guild Id set to {guildId}";
                    _messageBuilder.Color= DiscordColor.Green;

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder()
                    ));
                }
        }

        [SlashCommand("setraidchannel", "Set the raid channel")]
        public async Task SetRaidChannelCommand(InteractionContext ctx, [Option("channel", "The channel to set")] DiscordChannel channel)
        {
            ulong guildId = ctx.Guild.Id;

            _messageBuilder.Title = "Guild Id has already been set";
            _messageBuilder.Color= DiscordColor.Red;
                    
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(_messageBuilder.EmbedBuilder()));

            try
            {
                if (!await _guildSettings.SetRaidChannelId(guildId, channel.Id))
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Please set the guild id first")
                        .WithColor(DiscordColor.Red)
                    ));

                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Set raid channel to " + channel.Name)
                        .WithColor(DiscordColor.Green)
                    ));
                }
            }
            catch (Exception e)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Error: " + e.Message)
                    .WithColor(DiscordColor.Red)
                ));
            }
        }
    }
}