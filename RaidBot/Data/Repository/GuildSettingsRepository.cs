using RaidBot.entities;


namespace RaidBot.Data.Repository
{
    public class GuildSettingsRepository : IGuildSettingsRepository
    {
        private DataContext? _context;
        public GuildSettingsRepository(DataContext ctx)
        {
            _context = ctx;
        }

        public async Task<bool> AddGuildId(ulong guildId)
        {
            if (_context.GuildSettings.Any(x => x.GuildId == guildId)) return false;

            GuildSettings guildSettings = new GuildSettings
            {
                GuildId = guildId
            };

            await _context.GuildSettings.AddAsync(guildSettings);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetRaidChannelId(ulong guildId, ulong raidChannelId)
        {
            if (!_context.GuildSettings.Any(x => x.GuildId == guildId)) return false;

            GuildSettings guildSettings = _context.GuildSettings.First(x => x.GuildId == guildId);
            guildSettings.RaidChannelId = raidChannelId;

            _context.GuildSettings.Update(guildSettings);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}