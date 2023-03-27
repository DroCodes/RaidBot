namespace RaidBot.Data.Repository
{
    public interface IRaidSettingsRepository
    {
        Task<bool> SaveNewRaid(string raidName, ulong guildId);
        Task<bool> DeleteRaid(string raidName, ulong guildId);
    }
}