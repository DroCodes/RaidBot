using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class UserRaidHistory
    {
        public int Id { get; set; }
        public int GuildMemberId { get; set; }
        public int TotalRaidCount { get; set; }
        public int TotalRaidCountLastMonth { get; set; }
        public DateTime LastRaidDate { get; set; }
        public List<RaidStatsByTier>? RaidStatsByTier { get; set; }
        public List<RaidStatsByRole>? RaidStatsByRole { get; set; }

        [ForeignKey("GuildMemberId")]
        public GuildMember GuildMember { get; set; }
    }
}