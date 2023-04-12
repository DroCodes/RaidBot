using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public interface IRaidSettingsRepository
    {
        Task<bool> SaveNewRaid(string raidName, ulong guildId);
        Task<bool> DeleteRaid(string raidName, ulong guildId);
        List<RaidSettings>? GetActiveRaids(ulong guildId);
        Task<bool> SaveRaidInfo(string raidName, string info);
        Task<bool> SaveRaidTier(string raidName, string tier);
        Task<bool> SaveDateTime(string raidName, DateTime date);
        Task<RaidSettings?> GetStatus(string raidName);
        Task<bool> SetRolesForRaid(string raidName, int[] roles);
        Task<List<AssignedTierRoles>> GetRoles(ulong guildId, string tierRole);
    }
}