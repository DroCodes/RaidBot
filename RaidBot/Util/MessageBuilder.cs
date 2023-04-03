using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace RaidBot.Util;

public class MessageBuilder : ApplicationCommandModule, IMessageBuilder
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DiscordColor Color { get; set; }

    public DiscordEmbedBuilder EmbedBuilder()
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = Title,
            Description = Description,
            Color = Color
        };

        return embed;
    }
}