using System.ComponentModel.DataAnnotations.Schema;

namespace RaidBot.entities
{
    public class ActiveRaids
    {
        public int id { get; set; }
        public List<RaidSettings>? ActiveRaidsList { get; set; }
        public ulong GuildId { get; set; }

        [ForeignKey("GuildId")]
        public GuildSettings? GuildSettings { get; set; }
    }
}