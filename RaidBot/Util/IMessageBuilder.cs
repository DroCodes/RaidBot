using DSharpPlus.Entities;

namespace RaidBot.Util;


    public interface IMessageBuilder
    {
        string Title { get; set; }
        string Description { get; set; }
        DiscordColor Color { get; set; }

        DiscordEmbedBuilder EmbedBuilder();
    }