using DSharpPlus.Entities;

namespace RaidBot.entities;

public class OverFlowRoster
{
    public int Id { get; set; }
    public DiscordMember? Member { get; set; }
    public DiscordRole? Role { get; set; }
    public int OverflowSettingsId{ get; set; }
    public RaidSettings? RaidSettings { get; set; }
}