using DSharpPlus.Entities;

namespace RaidBot.entities;

public class Roster
{
    public int Id { get; set; }
    public DiscordUser GroupMember { get; set; }
    public DiscordMember? Member { get; set; }
    public DiscordRole? Role { get; set; }
    public int RosterSettingsId { get; set; }
    public RaidSettings? RaidSettings { get; set; }
}