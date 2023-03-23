namespace RaidBot.entities
{
    public class GuildSettings
    {
        public int Id { get; set; }
        public ulong? GuildId { get; set; }
        public ulong? RaidChannelId { get; set; }
    }
}