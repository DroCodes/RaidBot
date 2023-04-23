using DSharpPlus.Entities;
using RaidBot.Data.Repository;
using RaidBot.entities;

namespace RaidBot.Util;

public class RaidRosterManagement
{
    private readonly IRosterRepository _roster;

    public RaidRosterManagement(IRosterRepository roster)
    {
        _roster = roster;
    }

    public async Task<Roster?> AddMemberToRoster(DiscordUser member)
    {
        try
        {
            var addMember = await _roster.AddMemberToRoster(967268051001159690, "test-raid", member, "tank");

            return addMember;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in raid roster management " + e);
            throw;
        }
    }
}