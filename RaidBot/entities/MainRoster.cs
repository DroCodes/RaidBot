using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RaidBot.entities;

public class MainRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public string Role { get; set; }

    
    public int MainRosterId { get; set; }

    [ForeignKey("MainRosterId")]
    public Roster Roster { get; set; }
}