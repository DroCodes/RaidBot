using DSharpPlus.Entities;

namespace RaidBot.entities;

public class BackUpRoster
{
    public int Id { get; set; }
    public int BackUpSettingsId{ get; set; }
    public DiscordMember? Member { get; set; }
    public DiscordRole? Role { get; set; }
    public RaidSettings? RaidSettings { get; set; }
}