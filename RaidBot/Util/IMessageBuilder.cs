using DSharpPlus.Entities;

namespace RaidBot.Util;


    public interface IMessageBuilder
    {
        DiscordEmbedBuilder EmbedBuilder(string title = null, string description = null, DiscordColor? color = null);
    }