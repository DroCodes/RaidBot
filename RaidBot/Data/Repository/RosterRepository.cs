using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class RosterRepository : IRosterRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    public RosterRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }


    public async Task<Roster?> AddMemberToRoster(ulong guildId, string raidName, DiscordMember member, string role)
    {
        try
        {
            var findGuild = await _context.GuildSettings
                .Include(x => x.RaidList)
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (findGuild?.RaidList == null)
            {
                return null;
            }
    
            var findRaid = findGuild.RaidList.FirstOrDefault(x => x.RaidName == raidName);

            var getRosterList = await _context.RaidSettings.Include(x => x.Roster)
                .FirstOrDefaultAsync(x => x.RaidName == raidName && x.GuildId == guildId);

            Roster roster;
            if (getRosterList == null)
            {
                roster = new Roster()
                {
                    RaidName = raidName
                };
                _context.Rosters.Add(roster);
                await _context.SaveChangesAsync();
            }
            else
            {
                roster = getRosterList.Roster ?? new Roster();
            }

            var mainRoster = new MainRoster()
            {
                MemberName = member.Username,
                MemberId = member.Id,
                Role = role,
                MainRosterId = roster.Id,
            };

            roster.MainRoster ??= new List<MainRoster>();
            roster.MainRoster?.Add(mainRoster);
            _context.MainRosters.Add(mainRoster);

            return (await _context.SaveChangesAsync() > 0 ? roster : null)!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving roster");
            throw;
        }
    }

}