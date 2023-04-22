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
        public Roster? Roster { get; set; }

        public GuildSettings GuildSettings { get; set; }
    }

}