using DSharpPlus.Entities;
using RaidBot.entities;

namespace RaidBot.Data.Repository;

public interface IRosterRepository
{
    public Task<Roster?> AddMemberToRoster(ulong guildId, string raidName, DiscordUser member, string role);
}