using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.entities;

namespace RaidBot.Commands
{
    public class TierSettingsCommands : ApplicationCommandModule
    {
        private static ITierSettingsRepository? _repo;
        public TierSettingsCommands(ITierSettingsRepository repo)
        {
            _repo = repo;
        }

        [SlashCommand("CreateTier", "Create a tier for a raid")]
        public async Task CreateNewTierCommand(InteractionContext ctx, [Option("Tier", "Add new tier")] double tier, [Option("Role", "Add role to tier")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;
            int convertedTier = (int)tier;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Creating Tier")
                    .WithDescription("Standby")
                    .WithColor(DiscordColor.Orange)
                ));

            try
            {
                var saveTier = await _repo.CreateTierRole(convertedTier, guildId, roleName);
                if (!saveTier)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Error")
                            .WithDescription("Tier already exists")
                            .WithColor(DiscordColor.Red)
                        ));
                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Tier Created")
                            .WithDescription("Tier Successfully Created")
                            .WithColor(DiscordColor.Green)
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
        }

        [SlashCommand("addroletotier", "Update a tier")]
        public async Task AddRoleToTierCommand(InteractionContext ctx, [Option("Tier", "Tier to update")] double tier, [Option("Role", "Role to update")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;
            int convertedTier = (int)tier;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Updating Tier")
                    .WithDescription("Standby")
                    .WithColor(DiscordColor.Orange)
                ));

            try
            {
                var updateTier = await _repo.AddRoleToTier(convertedTier, guildId, roleName);

                if (!updateTier)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Error")
                            .WithDescription("Tier does not exist")
                            .WithColor(DiscordColor.Red)
                        ));
                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Tier Updated")
                            .WithDescription("Tier Successfully Updated")
                            .WithColor(DiscordColor.Green)
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
        }

        [SlashCommand("removerolefromtier", "Remove a role from a tier")]
        public async Task RemoveRoleFromTier(InteractionContext ctx, [Option("Tier", "Tier to update")] double tier, [Option("Role", "Role to update")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;
            int convertedTier = (int)tier;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Updating Tier")
                    .WithDescription("Standby")
                    .WithColor(DiscordColor.Orange)
                ));

            try
            {
                var updateTier = await _repo.RemoveRoleFromTier(convertedTier, guildId, roleName);

                if (!updateTier)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Error")
                            .WithDescription("Tier does not exist")
                            .WithColor(DiscordColor.Red)
                        ));
                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle("Tier Updated")
                            .WithDescription("Tier Successfully Updated")
                            .WithColor(DiscordColor.Green)
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
        }

        [SlashCommand("tierlist", "List of all tiers")]
        public Task GetTierListCommand(InteractionContext ctx)
        {
            ulong guildId = ctx.Guild.Id;

            try
            {
                List<TierRole> tierList = _repo.GetAllTiers(guildId);

                var embed = new DiscordEmbedBuilder()
                    .WithTitle("Tier List")
                    .WithColor(DiscordColor.Green);

                foreach (var tier in tierList)
                {
                    var getRoles = _repo.GetRolesFromTier(tier.Id, guildId);
                    string roles = "";
                    foreach (var role in getRoles)
                    {
                        roles += $"{role}\n";
                    }
                    embed.AddField($"Tier {tier.Tier}", roles);
                }

                return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .AddEmbed(embed));
            }
            catch (Exception e)
            {
                return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Error")
                        .WithDescription(e.Message)
                        .WithColor(DiscordColor.Red)
                    ));
            }
        }

        [SlashCommand("deletetier", "delete a tier")]
        public async Task DeleteTierCommand(InteractionContext ctx, [Option("Tier", "Tier to be deleted")] double tier)
        {
            ulong guildId = ctx.Guild.Id;
            int convertedInt = (int)tier;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Deleting...")
                        .WithDescription("Stand by")
                        .WithColor(DiscordColor.Orange)));
            try
            {
                var deleteTier = _repo.DeleteTier(convertedInt, guildId);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(new DiscordEmbedBuilder().WithTitle("Success")
                    .WithDescription($"Tier {tier} deleted")
                    .WithColor(DiscordColor.Green)));
            } catch(Exception e)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                   .AddEmbed(new DiscordEmbedBuilder().WithTitle("Error")
                   .WithDescription($"Error: {e}")
                   .WithColor(DiscordColor.Red)));
            }
        }
    }
}