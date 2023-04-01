namespace RaidBot.entities
{
    public class GuildSettings
    {
        public ulong GuildId { get; set; }
        public ulong? RaidChannelId { get; set; }
        public ulong? RaidChannelGroup {get; set;}
    }
}