namespace RaidBot.Data.Repository
{
    public interface IGuildSettingsRepository
    {
        Task<bool> AddGuildId(ulong guildId);
        Task<bool> SetRaidChannelId(ulong guildId, ulong raidChannelId);
    }
}