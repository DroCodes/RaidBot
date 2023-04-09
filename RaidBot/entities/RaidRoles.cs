namespace RaidBot.entities;

public class RaidRoles
{
    public int Id { get; set; }
    public int RoleSettingsId{ get; set; }
    public int? TankRole { get; set; }
    public int? HealerRole { get; set; }
    public int? DpsRole { get; set; }
    public RaidSettings RaidSettings { get; set; }
}