using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;

namespace RaidBot.Commands
{
    public class GuildSettingsCommands : ApplicationCommandModule
    {
        private static IGuildSettingsRepository? _guildSettings;
        public GuildSettingsCommands(IGuildSettingsRepository guildSettingsRepository)
        {
            _guildSettings = guildSettingsRepository;
        }

        [SlashCommand("setguildid", "Set the guild id")]
        public static async Task SetGuildIdCommand(InteractionContext ctx)
        {
            ulong guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Seting guild id to " + guildId)
                    .WithColor(DiscordColor.Green)
                ));

            try
            {
                if (!await _guildSettings.AddGuildId(guildId))
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("The Guild Id has already been set")
                        .WithColor(DiscordColor.Red)
                    ));

                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Set guild id to " + guildId)
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

        [SlashCommand("setraidchannel", "Set the raid channel")]
        public static async Task SetRaidChannelCommand(InteractionContext ctx, [Option("channel", "The channel to set")] DiscordChannel channel)
        {
            ulong guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Seting raid channel to " + channel.Name)
                .WithColor(DiscordColor.Green)
            ));

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