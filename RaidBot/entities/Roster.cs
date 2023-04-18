using DSharpPlus.Entities;

namespace RaidBot.entities;

public class Roster
{
    public int Id { get; set; }
    public ICollection<Roster>? MainRoster { get; set; }
    public ICollection<OverFlowRoster>? OverFlowRoster { get; set; }
    public ICollection<BackUpRoster>? BackUpRoster { get; set; }
    public int RosterSettingsId { get; set; }
    public RaidSettings? RaidSettings { get; set; }
}