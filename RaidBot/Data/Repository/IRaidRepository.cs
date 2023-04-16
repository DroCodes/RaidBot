using RaidBot.entities;

namespace RaidBot.Data.Repository;

public interface IRaidRepository
{
    Task<bool> CreateRaid(string raidName, ulong guildId);
    Task<bool> DeleteRaid(string raidName, ulong guildId);
    List<RaidSettings>? GetActiveRaids(ulong guildId);
    Task<RaidSettings?> GetStatus(string raidName);
}