using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
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
                    .FirstOrDefaultAsync(x => x.RaidName == raidName);

                if (existingRaid != null)
                {
                    // Raid name already exists
                    return false;
                }

                var guildSettings = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == guildId);

                if (guildSettings == null)
                {
                    // Guild settings not found, return error
                    return false;
                }

                var newRaid = new ActiveRaids()
                {
                    GuildId = guildId,
                    Raids = new List<RaidSettings>()
                    {
                        new()
                        {
                            RaidName = raidName,
                            GuildId = guildId,
                        }
                    }
                };

                _context.ActiveRaids.Add(newRaid);
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
                var raid = _context.RaidSettings.FirstOrDefault(x => x.RaidName == raidName);

                if (raid == null)
                {
                    return false;
                }

                var activeRaid = _context.ActiveRaids.FirstOrDefault(x => x.Id == raid.ActiveRaidId);

                if (activeRaid == null)
                {
                    return false;
                }


                _context.ActiveRaids.Remove(activeRaid);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting raid");
                return false;
            }
        }

        public async Task<List<RaidSettings>>? GetActiveRaids(ulong guildId)
        {
            try
            {
                var getRaidList = _context.RaidSettings.Where(x => x.GuildId == guildId).OrderBy(x => x.RaidName)
                    .ToList();

                if (getRaidList == null) return null;

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
                var findRaidWithRaidName = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

                if (findRaidWithRaidName == null)
                {
                    return false;
                }

                var findActiveRaids =
                    await _context.ActiveRaids.FirstOrDefaultAsync(x => x.Id == findRaidWithRaidName.ActiveRaidId);

                if (findActiveRaids == null)
                {
                    return false;
                }

                findRaidWithRaidName.Info = info;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error adding info");
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

                var findActiveRaid = await _context.ActiveRaids.FirstOrDefaultAsync(x => x.Id == findRaid.ActiveRaidId);

                if (findActiveRaid == null)
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
                if (findRaid == null) return false;

                var findActiveRaid = await _context.ActiveRaids.FirstOrDefaultAsync(x => x.Id == findRaid.ActiveRaidId);

                if (findActiveRaid == null) return false;

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
                var raid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

                if (raid == null)
                {
                    return false;
                }

                var findRaid = _context.ActiveRaids.FirstOrDefaultAsync(x => x.Id == raid.ActiveRaidId);

                if (findRaid == null)
                {
                    return false;
                }

                int tank = roles[0];
                int healer = roles[1];
                int dps = roles[2];

                RaidRoles newRoles = new RaidRoles()
                {
                    TankRole = tank,
                    HealerRole = healer,
                    DpsRole = dps,
                    RoleSettingsId = raid.Id // set the foreign key property
                };

                raid.Roles = newRoles;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was an error saving the roles");
                return false;
            }
        }

        public async Task<RaidSettings> GetStatus(string raidName)
        {
            try
            {
                var getRaid = await _context.RaidSettings.FirstOrDefaultAsync(x => x.RaidName == raidName);

                if (getRaid == null)
                {
                    return null;
                }

                var raidStatus = _context.ActiveRaids.FirstOrDefault(x => x.Id == getRaid.ActiveRaidId);

                if (raidStatus == null)
                {
                    return null;
                }

                DateTime? date = getRaid.Date;
                TimeSpan? time = getRaid.Time;

                var combinedDateTime = date + time;

                if (combinedDateTime == null)
                {
                    var newDate = new DateTime(2000, 01, 01);
                    combinedDateTime = newDate;
                }

                RaidSettings raidStats = new RaidSettings()
                {
                    RaidName = getRaid.RaidName,
                    Info = getRaid.Info,
                    TierRole = getRaid.TierRole,
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