using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;


namespace RaidBot.Data.Repository
{
    public class GuildSettingsRepository : IGuildSettingsRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        public GuildSettingsRepository(DataContext ctx, ILogger logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public async Task<bool> AddGuildId(ulong guildId)
        {
            try
            {
                var checkGuildExistsInDb = _context.GuildSettings.SingleOrDefault(x => x.GuildId == guildId);
                if (checkGuildExistsInDb != null) return false;
            
            var guildSettings = new GuildSettings
            {
                GuildId = guildId
            };

                _context.GuildSettings.Add(guildSettings);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Saving GuildId in AddGuildId method");
                return false;
            }
           
            return true;
        }

        public async Task<bool> SetRaidChannelId(ulong guildId, ulong raidChannelId)
        {
            try
            {
                var checkGuildExistsInDb = _context.GuildSettings.SingleOrDefault(x => x.GuildId == guildId);
                if (checkGuildExistsInDb == null) return false;
                
                var guildSettings = _context.GuildSettings.FirstOrDefault(x => x.GuildId == guildId);
                if (guildSettings == null)
                {
                    return await _context.SaveChangesAsync() > 0;
                }
                
                guildSettings.RaidChannelId = raidChannelId;

                _context.GuildSettings.Update(guildSettings);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Saving RaidChannelId in SetRaidChannel method");
                return false;
            }
        }

        public async Task<bool> SetRaidChannelGroup(ulong guildId, ulong? channelGroupId)
        {
            
            try
            {
                var checkGuildExistsInDb = _context.GuildSettings.SingleOrDefault(x => x.GuildId == guildId);
                if (checkGuildExistsInDb == null) return false;

                var guildSettings = _context.GuildSettings.FirstOrDefault(x => x.GuildId == guildId);
                if (guildSettings == null)
                {
                    return await _context.SaveChangesAsync() > 0;
                }
                guildSettings.RaidChannelGroup = channelGroupId;

                _context.GuildSettings.Update(guildSettings);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error setting channel group");
                return false;
            }
        }

        public async Task<GuildSettings?> CheckGuildSettings(ulong guildId)
        {
            try
            {
                var checkGuildExists = await _context.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == guildId);
                if (checkGuildExists == null)
                {
                    return null;
                }
                
                var guildSettings = new GuildSettings()
                {
                    GuildId = checkGuildExists.GuildId,
                    RaidChannelId = checkGuildExists.RaidChannelId,
                    RaidChannelGroup = checkGuildExists.RaidChannelGroup
                };

                return guildSettings;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving guild settings");
                return null;
            }
            
        }
    }
}