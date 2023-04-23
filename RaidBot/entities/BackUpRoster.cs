using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus.Entities;

namespace RaidBot.entities;

public class BackUpRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public string Role { get; set; }
    public int BackUpRosterId { get; set; }

    [ForeignKey("BackUpRosterId")]
    public Roster Roster { get; set; }
}