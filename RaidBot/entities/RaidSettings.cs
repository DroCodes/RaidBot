namespace RaidBot.entities
{
    public class RaidSettings
    {
        public int Id { get; set; }
        public string RaidName { get; set; }
        public ulong GuildId { get; set; }
        public string? TierRole { get; set; }
        public string? Info { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public int ActiveRaidId { get; set; }

        public ActiveRaids ActiveRaids { get; set; }
    }

}