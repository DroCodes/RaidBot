using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository
{
    public class TierSettingsRepository : ITierSettingsRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public TierSettingsRepository(DataContext ctx, ILogger logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public async Task<bool> CreateTierRole(string tier, ulong guildId, string roleName)
        {
            try
            {
                var id = _context.GuildSettings.FirstOrDefault(g => g.GuildId == guildId);
                if (id == null) return false;
                var tierRole = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == id.GuildId);
                if (tierRole != null) return false;
                var newTier = new TierRole
                {
                    TierName = tier,
                    GuildId = guildId,
                    Roles = new List<AssignedTierRoles>
                    {
                        new()
                        {
                            RoleName = roleName
                        }
                    }
                };

                _context.TierRoles.Add(newTier);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong creating the tier");
                return false;
            }
        }


        public async Task<bool> AddRoleToTier(string tier, ulong guildId, string roleName)
        {
            try
            {
                var tierRole = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == guildId);

                if (tierRole == null) return false;

                tierRole.Roles ??= new List<AssignedTierRoles>();

                tierRole.Roles.Add(new AssignedTierRoles()
                {
                    RoleName = roleName
                });

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding role to tier");
                return false;
            }
        }

        public async Task<bool> RemoveRoleFromTier(string tier, ulong guildId, string roleName)
        {
            try
            {
                var tierRole = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == guildId);

                if (tierRole == null)
                {
                    return false;
                }

                var role = tierRole.Roles.FirstOrDefault(x => x.RoleName == roleName);

                if (role == null)
                {
                    return false;
                }

                var discordRole = _context.AssignedTierRoles.FirstOrDefault(x => x.RoleName == roleName);
                if (discordRole == null)
                {
                    return false;
                }

                _context.AssignedTierRoles.Remove(discordRole);

                tierRole.Roles.Remove(role);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"There was an error removing {roleName} from {tier}");
                return false;
            }
        }

        public List<TierRole>? GetAllTiers(ulong guildId)
        {
            try
            {
                var allTiers = _context.TierRoles.Where(x => x.GuildId == guildId).ToList();
                if (allTiers == null) return null;
                return allTiers;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was an error retrieving the tiers");
                return null;
            }
        }


        public List<string>? GetRolesFromTier(int id, ulong guildId)
        {
            try
            {
                var roles = _context.AssignedTierRoles.Where(x => x.TierRoleId == id);
                List<string> roleList = new List<string>();

                if (roles == null)
                {
                    return null;
                }

                foreach (var role in roles)
                {
                    roleList.Add(role.RoleName);
                }

                return roleList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"There was an error getting the roles from tier");
                return null;
            }
        }

        public async Task<bool> DeleteTier(string tier, ulong guildId)
        {
            try
            {
                var getTier = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == guildId);
                var getRoles = _context.AssignedTierRoles.Where(x => x.TierRoleId == getTier.Id);

                if (getTier == null || getRoles == null) return false;

                foreach (var role in getRoles)
                {
                    _context.AssignedTierRoles.Remove(role);
                }

                _context.TierRoles.Remove(getTier);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deleting {tier}");
                return false;
            }
        }

        public async Task<List<AssignedTierRoles>> GetRoles(ulong guildId, string tierName)
        {
            try
            {
                var getTier = await _context.TierRoles.FirstOrDefaultAsync(x => x.TierName == tierName);
                if (getTier == null)
                {
                    return null;
                }

                var getRoles = _context.AssignedTierRoles.Where(x => x.TierRoleId == getTier.Id).ToList();

                // var roles = new List<AssignedTierRoles>();
                //
                // foreach (var role in getRoles)
                // {
                //     roles.Add(role);
                //     Console.WriteLine(role);
                // }

                return getRoles;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting the roles in GetRoles");
                return null;
            }
        }
    }
}