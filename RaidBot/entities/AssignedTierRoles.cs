using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class AssignedTierRoles
    {
        public int Id { get; set; }
        public int AssignedTierRoleId { get; set; }
        public string? RoleName { get; set; }

        [ForeignKey("TierRoleId")]
        public TierRole? TierRole { get; set; }
    }
}