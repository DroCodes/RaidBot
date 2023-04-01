using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public class RaidSettingsRepository : IRaidSettingsRepository
    {
        private readonly DataContext _context;
        public RaidSettingsRepository(DataContext ctx)
        {
            _context = ctx;
        }

        public async Task<bool> SaveNewRaid(string raidName, ulong guildId)
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

        public async Task<bool> DeleteRaid(string raidName, ulong guildId)
        {
            var raid = _context.RaidSettings.FirstOrDefault(x => x.RaidName == raidName && x.GuildId == guildId);

            if (raid == null)
            {
                return false;
            }

            _context.RaidSettings.Remove(raid);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}