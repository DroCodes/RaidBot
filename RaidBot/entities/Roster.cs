using System.ComponentModel.DataAnnotations.Schema;
using DSharpPlus.Entities;

namespace RaidBot.entities;

public class Roster
{
    public int Id { get; set; }
    public string RaidName { get; set; }
    public ICollection<MainRoster>? MainRoster { get; set; }
    public ICollection<OverFlowRoster>? OverFlowRoster { get; set; }
    public ICollection<BackUpRoster>? BackUpRoster { get; set; }
    public int RosterSettingsId { get; set; }
    [ForeignKey("RosterSettingsId")]
    public RaidSettings RaidSettings { get; set; }
}