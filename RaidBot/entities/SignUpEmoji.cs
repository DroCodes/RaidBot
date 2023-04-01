using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class SignUpEmoji
    {
        public ulong Id { get; set; }
        public int RaidSettingsId { get; set; }
        public ulong? AssignedRole { get; set; } 
        public string? Name { get; set; }   
        public string? Url { get; set; }

        [ForeignKey("RaidSettingsId")]
        public RaidSettings? RaidSettings { get; set; }
    }
}

