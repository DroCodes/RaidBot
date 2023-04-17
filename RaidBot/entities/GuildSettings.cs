namespace RaidBot.entities
{
    public class GuildSettings
    {
        public ulong GuildId { get; set; }
        public ulong? RaidChannelId { get; set; }
        public ulong? RaidChannelGroup {get; set;}
        public List<SignUpEmoji>? Emoji { get; set; }
        public ICollection<RaidSettings>? RaidList { get; set; }
    }
}