using Microsoft.EntityFrameworkCore;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class RaidInfoRepository : IRaidInfoRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    public RaidInfoRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }
    
    public async Task<bool> SaveRaidInfo(string raidName, string info)
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

            findRaid.Info = info;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding info");
            return false;
        }
    }

    public async Task<bool> SaveRaidTier(string raidName, string tier)
    {
        try
        {
            var findRaid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

            if (findRaid == null)
            {
                return false;
            }

            var findTier = await _context.TierRoles.FirstOrDefaultAsync(x => x.TierName == tier);

            if (findTier == null)
            {
                return false;
            }

            var findGuild = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == findRaid.GuildId);

            if (findGuild == null)
            {
                return false;
            }

            findRaid.TierRole = tier;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "there was an issue saving the tier");
            return false;
        }
    }

    public async Task<bool> SaveDateTime(string raidName, DateTime date)
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

            findRaid.Date = date.Date;
            findRaid.Time = date.TimeOfDay;

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error saving the date");
            return false;
        }
    }
}