using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public interface ITierSettingsRepository
    {
        Task<bool> CreateTierRole(string tier, ulong guildId, string roleName);
        Task<bool> AddRoleToTier(string tier, ulong guildId, string roleName);
        Task<bool> RemoveRoleFromTier(string tier, ulong guildId, string roleName);
        List<TierRole>? GetAllTiers(ulong guildId);
        List<string> GetRolesFromTier(int tierId, ulong guildId);
        Task<bool> DeleteTier(string tier, ulong guildId);
        Task<List<AssignedTierRoles>> GetRoles(ulong guildId, string tierName);
    }
}