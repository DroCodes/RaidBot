using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class SignUpEmoji
    {
        public ulong Id { get; set; }
        public ulong GuildSettingsId { get; set; }
        public ulong? AssignedRole { get; set; } 
        public string? Name { get; set; }   
        public string? Url { get; set; }
        
        public GuildSettings? GuildSettings { get; set; }
    }
}

