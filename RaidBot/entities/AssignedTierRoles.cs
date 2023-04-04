using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class AssignedTierRoles
    {
        public int Id { get; set; }
        public int TierRoleId { get; set; }
        public string? RoleName { get; set; }

        public TierRole TierRole { get; set; }
    }
}