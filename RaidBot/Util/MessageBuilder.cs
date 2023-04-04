using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace RaidBot.Util;

public class MessageBuilder : ApplicationCommandModule, IMessageBuilder
{
    public DiscordEmbedBuilder EmbedBuilder(string title = null, string description = null, DiscordColor? color = null)
    {
        var embed = new DiscordEmbedBuilder();
        if (title != null)
        {
            embed.Title = title;
        }
    
        if (description != null)
        {
            embed.Description = description;
        }
    
        if (color != null)
        {
            embed.Color = color.Value;
        }
        return embed;
    }

}