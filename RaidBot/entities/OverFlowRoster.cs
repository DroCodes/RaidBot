using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus.Entities;

namespace RaidBot.entities;

public class OverFlowRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public int OverFlowRosterId { get; set; }
    public string Role { get; set; }


    [ForeignKey("OverFlowRosterId")]
    public Roster Roster { get; set; }
}