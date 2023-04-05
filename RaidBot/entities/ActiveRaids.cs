namespace RaidBot.entities
{
    public class ActiveRaids
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public GuildSettings? GuildSettings { get; set; }
        public List<RaidSettings>? Raids { get; set; }
    }
}