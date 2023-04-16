using DSharpPlus;
using DSharpPlus.Entities;

namespace RaidBot.Util;

public class ChannelManager
{
    public async Task<DiscordChannel> CreateRaidChannelAsync(DiscordGuild guild, string name, ChannelType type,
        DiscordChannel parent)
    {
        var channel = guild.Channels.Values;
        foreach (var c in channel)
        {
            if (c.Name == name)
            {
                return null;
            }
        }

        var newChannel = await guild.CreateChannelAsync(name, type, parent);
        return newChannel;
    }

    public async Task<DiscordThreadChannel> CreateRaidChannelThread(DiscordChannel channel, string name, ChannelType threadType)
    {
        var newThread = await channel.CreateThreadAsync(name, AutoArchiveDuration.Week, threadType);

        return newThread;
    }
}