namespace RaidBot.entities;

public class MainRoster
{
    public int Id { get; set; }
    public string MemberName { get; set; }
    public ulong MemberId { get; set; }
    public int MainRosterId { get; set; }

    public RaidSettings RaidSettings { get; set; }
}