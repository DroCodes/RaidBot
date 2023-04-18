using DSharpPlus.Entities;

namespace RaidBot.entities;

public class BackUpRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public int BackUpRosterId { get; set; }

    public RaidSettings RaidSettings { get; set; }
}