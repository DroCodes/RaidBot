namespace RaidBot.entities
{
    public class TierRole
    {
        public int Id { get; set; }
        public int Tier { get; set; }
        public ulong GuildId { get; set; }
        public List<DiscordRoles>? Roles { get; set; }
    }
}