using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands.RaidCommands
{
    public class RaidCreationCommands : ApplicationCommandModule
    {
        private readonly IRaidSettingsRepository? _repo;
        private readonly IMessageBuilder _messageBuilder;
        private const string InitialResponse = "Thinking";
        private string? _title;
        private string? _description;
        private DiscordColor _color;

        public RaidCreationCommands(IRaidSettingsRepository repo, IMessageBuilder messageBuilder)
        {
            _repo = repo;
            _messageBuilder = messageBuilder;
        }

        [SlashCommand("createraid", "Create a raid")]
        public async Task CreateRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
        {
            var guildId = ctx.Guild.Id;
            // send the user a private message
            var dm = await ctx.Member.CreateDmChannelAsync();

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)
                    ));

            if (!await _repo.SaveNewRaid(name, guildId))
            {
                _title = "Error";
                _description = "There was a problem creating the raid.";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
                return;
            }

            await dm.SendMessageAsync("Please continue with raid creation. \n" +
                                      "type /info /roles /tier /date /time");

            _title = "Success";
            _description = "Raid creation started, continue in a private message with me";
            _color = DiscordColor.Green;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
        }

        [SlashCommand("deleteraid", "Delete a raid")]
        public async Task DeleteRaid(InteractionContext ctx, [Option("raid-name", "The name of the raid")] string name)
        {
            var guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)
                    ));

            if (!await _repo?.DeleteRaid(name, guildId)!)
            {
                _title = "Error";
                _description = $"Something went wrong deleting {name}";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
                return;
            }

            _title = "Success";
            _description = $"{name} Successfully deleted";
            _color = DiscordColor.Green;
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                ));
        }

        [SlashCommand("raidlist", "Gets the list of active raids")]
        public async Task GetRaidListCommand(InteractionContext ctx)
        {
            var guildId = ctx.Guild.Id;

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(InitialResponse)
                    ));
            var getRaids = await _repo.GetActiveRaids(guildId);

            if (getRaids == null)
            {
                _title = "Error";
                _description = "There was a problem getting the raid list";
                _color = DiscordColor.Red;
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)
                    ));
                return;
            }

            _title = "Active Raids";
            _description = "";
            _color = DiscordColor.Green;

            foreach (var raid in getRaids)
            {
                var raidName = raid.RaidName;

                _description += $"{raidName}\n";
            }
            
            ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(_messageBuilder.EmbedBuilder(_title, _description, _color)));
        }
    }
}