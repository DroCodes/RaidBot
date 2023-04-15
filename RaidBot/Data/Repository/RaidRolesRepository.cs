using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class RaidRolesRepository : IRaidRolesRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    public RaidRolesRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }

    public async Task<bool> SetRolesForRaid(string raidName, int[] roles)
    {
        try
        {
            var findRaid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

            if (findRaid == null)
            {
                return false;
            }

            var findGuild = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == findRaid.GuildId);

            if (findGuild == null)
            {
                return false;
            }

            int tank = roles[0];
            int healer = roles[1];
            int dps = roles[2];

            var existingRoles = await _context.RaidRoles.FirstOrDefaultAsync(x => x.RoleSettingsId == findRaid.Id);

            RaidRoles? newRoles = null;

            if (existingRoles != null)
            {
                existingRoles.TankRole = tank;
                existingRoles.HealerRole = healer;
                existingRoles.DpsRole = dps;
            }
            else
            {
                newRoles = new RaidRoles()
                {
                    TankRole = tank,
                    HealerRole = healer,
                    DpsRole = dps,
                    RoleSettingsId = findRaid.Id // set the foreign key property
                };

                _context.RaidRoles.Add(newRoles);
            }

            findRaid.Roles = existingRoles ?? newRoles;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error saving the roles");
            return false;
        }
    }

    public async Task<List<AssignedTierRoles>> GetRoles(ulong guildId, string tierRole)
    {
        try
        {
            // var getGuild = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == guildId);
            var getTierRoles = await _context.AssignedTierRoles
                .Where(x => x.TierRole.TierName == tierRole && x.TierRole.Id == x.TierRoleId).ToListAsync();

            return getTierRoles;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting roles");
            return null;
        }
    }
}