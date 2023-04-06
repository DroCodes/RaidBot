using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Commands
{
    public class TierSettingsCommands : ApplicationCommandModule
    {
        private readonly ITierSettingsRepository _repo;
        private readonly IMessageBuilder _messageBuilder;
        private readonly string _initialResponse = "Thinking";
        private string? _title;
        private string? _description;
        private DiscordColor? _color;

        public TierSettingsCommands(ITierSettingsRepository repo, IMessageBuilder messageBuilder)
        {
            _repo = repo;
            _messageBuilder = messageBuilder;
        }

        [SlashCommand("createtier", "Create a tier for a raid")]
        public async Task CreateNewTierCommand(InteractionContext ctx,
            [Option("TierName", "Name for the Tier")]
            string tier,
            [Option("Role", "Role for the tier")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)
                    ));

            var saveTier = await _repo.CreateTierRole(tier, guildId, roleName);
            if (!saveTier)
            {
                _title = "Error";
                _description = "Tier already exists";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
            else
            {
                _title = "Success";
                _description = $"Tier {tier} successfully created";
                _color = DiscordColor.Green;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
        }

        [SlashCommand("addroletotier", "Update a tier")]
        public async Task AddRoleToTierCommand(InteractionContext ctx, [Option("Tier", "Tier to update")] string tier,
            [Option("Role", "Role to update")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)
                    ));

            var updateTier = await _repo.AddRoleToTier(tier, guildId, roleName);

            if (!updateTier)
            {
                _title = "Error";
                _description = "Tier does not exist";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
            else
            {
                _title = "Success";
                _description = "Tier successfully updated";
                _color = DiscordColor.Green;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
        }


        [SlashCommand("removerolefromtier", "Remove a role from a tier")]
        public async Task RemoveRoleFromTier(InteractionContext ctx, [Option("Tier", "Tier to update")] string tier,
            [Option("Role", "Role to update")] DiscordRole role)
        {
            ulong guildId = ctx.Guild.Id;
            string roleName = role.Name;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse))
            );


            var updateTier = await _repo.RemoveRoleFromTier(tier, guildId, roleName);

            if (!updateTier)
            {
                _title = "Error";
                _description = "Tier does not exist";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
            else
            {
                _title = "Success";
                _description = "Tier successfully updated";
                _color = DiscordColor.Green;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
            }
        }


        [SlashCommand("tierlist", "List of all tiers")]
        public async Task GetTierListCommand(InteractionContext ctx)
        {
            ulong guildId = ctx.Guild.Id;

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

                embed.AddField($"Tier {tier.TierName}", roles);
            }

            ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(embed));
        }

        [SlashCommand("deletetier", "delete a tier")]
        public async Task DeleteTierCommand(InteractionContext ctx, [Option("Tier", "Tier to be deleted")] string tier)
        {
            ulong guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_initialResponse)));

            var deleteTier = await _repo.DeleteTier(tier, guildId);
            if (!deleteTier)
            {
                _title = "Error";
                _description = "Something went wrong, please ensure the tier you are trying to delete exists";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)));
            }
            _title = "Success";
            _description = $"Tier {tier} deleted";
            _color = DiscordColor.Green;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)));
        }
    }
}