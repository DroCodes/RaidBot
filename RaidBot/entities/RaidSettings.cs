namespace RaidBot.entities
{
    public class RaidSettings
    {
        public int Id { get; set; }
        public string RaidName { get; set; }
        public ulong GuildId { get; set; }
        public string? TierRole { get; set; }
        public RaidRoles? Roles { get; set; }
        public string? Info { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public ICollection<Roster>? Roster { get; set; }
        public ICollection<OverFlowRoster>? OverFlow { get; set; }
        public ICollection<BackUpRoster>? BackUp { get; set; }

        public GuildSettings GuildSettings { get; set; }
    }

}