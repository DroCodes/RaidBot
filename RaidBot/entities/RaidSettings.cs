using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class RaidSettings
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public int RaidId { get; set; }
        public string? RaidName { get; set; }
        public string? TierRole { get; set; }
        public string? Info { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        // [ForeignKey("GuildId")]
        // public GuildSettings GuildSettings { get; set; }
        [ForeignKey("RaidId")]
        public ActiveRaids ActiveRaids { get; set; }
    }
}