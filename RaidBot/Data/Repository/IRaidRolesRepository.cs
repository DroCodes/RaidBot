using RaidBot.entities;

namespace RaidBot.Data.Repository;

public interface IRaidRolesRepository
{
    Task<bool> SetRolesForRaid(string raidName, int[] roles);
    Task<List<AssignedTierRoles>> GetRoles(ulong guildId, string tierRole);
}