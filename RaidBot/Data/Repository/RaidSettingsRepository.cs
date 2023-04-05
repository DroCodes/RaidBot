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
                var existingGuildRaid = await _context.ActiveRaids
                    .FirstOrDefaultAsync(x => x.GuildId == guildId);
                // Console.WriteLine("test");

                if (existingGuildRaid != null)
                {
                    // Guild already has an active raid
                    return false;
                }

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
                var activeRaid = _context.ActiveRaids.FirstOrDefault(x => x.GuildId == guildId);

                if (activeRaid == null)
                {
                    return false;
                }
                
                var raid = _context.RaidSettings.FirstOrDefault(xv => xv.RaidName == raidName);

                if (raid == null)
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

        public Task<bool> SaveRaidInfo(ulong guildId, string raidName, string info)
        {
            throw new NotImplementedException();
        }
    }
}