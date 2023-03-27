using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus.Entities;

namespace RaidBot.entities
{
    public class RaidSettings
    {
        public int Id { get; set; }
        [ForeignKey("GuildId")]
        public ulong GuildId { get; set; }
        public string? Name { get; set; }
        public DiscordUser? RaidLeader { get; set; }
        public List<DiscordUser>? member { get; set; }
        public string? Info { get; set; }
        public int Tank { get; set; }
        public int Healer { get; set; }
        public int Dps { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public int? Tier { get; set; }
    }
}