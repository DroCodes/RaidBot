using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus.Entities;

namespace RaidBot.entities
{
    public class GuildMember
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public DiscordMember? DiscordMember { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("GuildId")]
        public GuildSettings? GuildSettings { get; set; }
    }
}