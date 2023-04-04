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
                if (_context.RaidSettings.Any(x => x.RaidName == raidName) && _context.RaidSettings.Any(x => x.GuildId == guildId))
                {
                    return false;
                }

                RaidSettings raidSettings = new RaidSettings
                {
                    RaidName = raidName,
                    GuildId = guildId
                };
                _context.RaidSettings.Add(raidSettings);
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
                var raid = _context.RaidSettings.FirstOrDefault(x => x.RaidName == raidName && x.GuildId == guildId);

                if (raid == null)
                {
                    return false;
                }

                _context.RaidSettings.Remove(raid);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting raid");
                return false;
            }
          
        }
    }
}