using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class Roles
    {
        public int Id { get; set; }
        public int RaidSettingsId { get; set; }
        public string? Tank { get; set; }
        public string? Healer { get; set; }
        public string? Dps { get; set; }

        [ForeignKey("RaidSettingsId")]
        public RaidSettings? RaidSettings { get; set; }
    }
}