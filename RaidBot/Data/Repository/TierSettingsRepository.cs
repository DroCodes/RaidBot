using RaidBot.entities;

namespace RaidBot.Data.Repository
{
    public class TierSettingsRepository : ITierSettingsRepository
    {
        private DataContext _context;
        public TierSettingsRepository(DataContext ctx)
        {
            _context = ctx;
        }

        public async Task<bool> CreateTierRole(string tier, ulong guildId, string roleName)
        {
            var id = _context.GuildSettings.FirstOrDefault(g => g.GuildId == guildId);
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

        public async Task<bool> AddRoleToTier(string tier, ulong guildId, string roleName)
        {
            var tierRole = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == guildId);

            if (tierRole == null) return false;

            if (tierRole.Roles == null)
            {
                tierRole.Roles = new List<AssignedTierRoles>();
            }

            tierRole.Roles.Add(new AssignedTierRoles()
            {
                RoleName = roleName
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRoleFromTier(string tier, ulong guildId, string roleName)
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

        public List<TierRole>? GetAllTiers(ulong guildId)
        {
            var allTiers = _context.TierRoles.Where(x => x.GuildId == guildId).ToList();
            if (allTiers == null) return null;
            return allTiers;
        }

        public List<string>? GetRolesFromTier(int id, ulong guildId)
        {
            var roles = _context.AssignedTierRoles.Where(x => x.AssignedTierRoleId == id);
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

        public async Task<bool> DeleteTier(string tier, ulong guildId)
        {
            var getTier = _context.TierRoles.FirstOrDefault(x => x.TierName == tier && x.GuildId == guildId);
            var getRoles = _context.AssignedTierRoles.Where(x => x.AssignedTierRoleId == getTier.Id);

            if (getTier == null || getRoles == null) return false;

            foreach(var role in getRoles)
            {
                _context.AssignedTierRoles.Remove(role);
            }

            _context.TierRoles.Remove(getTier);

            return _context.SaveChanges() > 0;
        }
    }
}