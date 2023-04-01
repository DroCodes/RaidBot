using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class RaidStatsByTier
    {
        public int Id { get; set; }
        public int RaidHistoryId { get; set; }
        public string? TierName { get; set; }
        public int TierCount { get; set; }

        [ForeignKey("RaidHistoryId")]
        public UserRaidHistory UserRaidHistory { get; set; }

    }
}