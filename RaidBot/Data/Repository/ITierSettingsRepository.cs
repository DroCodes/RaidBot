using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public interface ITierSettingsRepository
    {
        Task<bool> CreateTierRole(int tier, ulong guildId, string roleName);
        Task<bool> AddRoleToTier(int tier, ulong guildId, string roleName);
        Task<bool> RemoveRoleFromTier(int tier, ulong guildId, string roleName);
        List<TierRole> GetAllTiers(ulong guildId);
        List<string> GetRolesFromTier(int tier, ulong guildId);
        Task<bool> DeleteTier(int tier, ulong guildId);
    }
}