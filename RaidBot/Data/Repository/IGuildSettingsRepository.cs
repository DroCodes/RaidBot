using DSharpPlus.Entities;

namespace RaidBot.Data.Repository
{
    public interface IGuildSettingsRepository
    {
        Task<bool> AddGuildId(ulong guildId);
        Task<bool> SetRaidChannelId(ulong guildId, ulong raidChannelId);
        Task<bool> SetRaidChannelGroup(ulong guildId, ulong? channelGroupId);
        Task<List<ulong?>> CheckGuildSettings(ulong guildId);
    }
}