using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;

namespace RaidBot.Commands.RaidCommands
{
    public class RaidCreationCommands : ApplicationCommandModule
    {
        private static IRaidSettingsRepository? _repo;
        public RaidCreationCommands(IRaidSettingsRepository repo)
        {
            _repo = repo;
        }

        [SlashCommand("create", "Create a raid")]
        public async Task CreateRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
        {
            var guildId = ctx.Guild.Id;
            // send the user a private message
            var dm = await ctx.Member.CreateDmChannelAsync();

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Creating Raid")
                    .WithDescription("Standby for a DM from me")
                    .WithColor(DiscordColor.Orange)
                ));
            try
            {
                if (!await _repo.SaveNewRaid(name, guildId))
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Error")
                            .WithDescription("Raid already exists")
                            .WithColor(DiscordColor.Red)
                        ));
                }
            }
            catch (Exception e)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Error")
                        .WithDescription(e.Message)
                        .WithColor(DiscordColor.Red)
                    ));
            }
            await dm.SendMessageAsync("Hello, world!");

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Raid Created")
                    .WithDescription("Raid creation is continued in a pivate message with me")
                    .WithColor(DiscordColor.Green)
                ));
        }

        [SlashCommand("delete", "Delete a raid")]
        public async Task DeleteRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
        {
            var guildId = ctx.Guild.Id;
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Deleting Raid")
                    .WithDescription("Standby")
                    .WithColor(DiscordColor.Orange)
                ));
            try
            {
                if (!await _repo.DeleteRaid(name, guildId))
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Error")
                            .WithDescription("Raid does not exist")
                            .WithColor(DiscordColor.Red)
                        ));
                }
            }
            catch (Exception e)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Error")
                        .WithDescription(e.Message)
                        .WithColor(DiscordColor.Red)
                    ));
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Raid Deleted")
                    .WithDescription("Raid Successfully Deleted")
                    .WithColor(DiscordColor.Green)
                ));
        }
    }
}