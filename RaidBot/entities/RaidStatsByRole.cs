using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class RaidStatsByRole
    {
        public int Id { get; set; }
        public int RaidHistoryId { get; set; }
        public string? Role { get; set; }
        public int Count { get; set; }

        [ForeignKey("RaidHistoryId")]
        public UserRaidHistory? UserRaidHistory { get; set; }
    }
}