using System.Runtime.Intrinsics.X86;
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
                            GuildId = guildId
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
    }
}