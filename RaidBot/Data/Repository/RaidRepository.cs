using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class RaidRepository : IRaidRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    public RaidRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }

    public async Task<bool> CreateRaid(string raidName, ulong guildId)
    {
        try
        {
            var findGuild = await _context.GuildSettings
                .Include(x => x.RaidList)
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (findGuild == null)
            {
                return false;
            }

            var existingRaid = findGuild.RaidList
                .FirstOrDefault(x => x.RaidName == raidName);

            if (existingRaid != null)
            {
                // Raid name already exists
                return false;
            }

            var newRaid = new RaidSettings()
            {
                GuildId = guildId,
                RaidName = raidName,
            };

            findGuild.RaidList.Add(newRaid);

            _context.RaidSettings.Add(newRaid);

            return await _context.SaveChangesAsync() > 0;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating raid");
            return false;
        }
    }

    public async Task<bool> DeleteRaid(string raidName, ulong guildId)
    {
        try
        {
            var findRaid =
                await _context.RaidSettings.FirstOrDefaultAsync(x =>
                    x.RaidName == raidName && x.GuildId == guildId);

            if (findRaid == null)
            {
                return false;
            }

            _context.RaidSettings.Remove(findRaid);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting raid");
            return false;
        }
    }

    public List<RaidSettings>? GetActiveRaids(ulong guildId)
    {
        try
        {
            var getRaidList = _context.RaidSettings.Where(x => x.GuildId == guildId).OrderBy(x => x.RaidName)
                .ToList();

            return getRaidList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "something went wrong retrieving the list of raids");
            return null;
        }
    }

    public async Task<RaidSettings?> GetStatus(string raidName)
    {
        try
        {
            var findRaid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

            if (findRaid == null)
            {
                return null;
            }

            var findGuild = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == findRaid.GuildId);

            if (findGuild == null)
            {
                return null;
            }

            var roles = await _context.RaidRoles.FirstOrDefaultAsync(x => x.RoleSettingsId == findRaid.Id);


            DateTime? date = findRaid.Date;
            TimeSpan? time = findRaid.Time;

            var combinedDateTime = date + time;

            if (combinedDateTime == null)
            {
                var newDate = new DateTime(2000, 01, 01);
                combinedDateTime = newDate;
            }

            var raidStats = new RaidSettings()
            {
                RaidName = findRaid.RaidName,
                GuildId = findRaid.GuildId,
                Info = findRaid.Info,
                TierRole = findRaid.TierRole,
                Roles = roles,
                Date = combinedDateTime
            };

            return raidStats;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}