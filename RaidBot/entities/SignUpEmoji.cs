using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class SignUpEmoji
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public string? RaidRole { get; set; } 
        public string? EmojiName { get; set; }

        [ForeignKey("GuildId")]
        public GuildSettings GuildSettings { get; set; }
    }
}

