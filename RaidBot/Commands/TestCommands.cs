using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace RaidBot.Commands
{
    public class TestCommands : ApplicationCommandModule
    {
        [SlashCommand("test", "Test command")]
        public static async Task TestCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Test command executed!"));
        }
    }
}