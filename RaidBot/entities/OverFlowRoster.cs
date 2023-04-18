using DSharpPlus.Entities;

namespace RaidBot.entities;

public class OverFlowRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public int OverFlowRosterId { get; set; }

    public RaidSettings RaidSettings { get; set; }
}