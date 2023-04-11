using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class TierRole
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public string? TierName { get; set; }
        public ICollection<AssignedTierRoles>? Roles { get; set; }

        [ForeignKey("GuildId")]
        public GuildSettings GuildSettings { get; set; }
    }
}