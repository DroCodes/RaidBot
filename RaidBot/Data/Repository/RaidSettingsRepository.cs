using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository
{
    public class RaidSettingsRepository : IRaidSettingsRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public RaidSettingsRepository(DataContext ctx, ILogger logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public async Task<bool> SaveNewRaid(string raidName, ulong guildId)
        {
            try
            {
                var existingRaid = await _context.RaidSettings
                    .FirstOrDefaultAsync(x => x.RaidName == raidName && x.GuildId == guildId);

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

                var activeRaid = new GuildSettings()
                {
                    RaidList = new List<RaidSettings>() { newRaid }
                };

                _context.RaidSettings.Add(newRaid);
                _context.GuildSettings.Add(activeRaid);
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
                var findRaid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName && x.GuildId == guildId);

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
                    Info = findRaid.Info,
                    TierRole = findRaid.TierRole,
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
}