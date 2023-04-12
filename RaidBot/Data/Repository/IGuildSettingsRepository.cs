using DSharpPlus.Entities;
using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public interface IGuildSettingsRepository
    {
        Task<bool> AddGuildId(ulong guildId);
        Task<bool> SetRaidChannelId(ulong guildId, ulong raidChannelId);
        Task<bool> SetRaidChannelGroup(ulong guildId, ulong? channelGroupId);
        Task<GuildSettings?> CheckGuildSettings(ulong guildId);
    }
}