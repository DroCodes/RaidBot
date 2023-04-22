using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RaidBot.Data.Repository;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Events;

public class ReactionSignUpEvent
{
    private readonly IGuildSettingsRepository _guildRepo;
    private readonly IRaidRepository _raidRepo;
    private readonly ITierSettingsRepository _tierRepo;
    private readonly IRosterRepository _roster;

    public ReactionSignUpEvent(IGuildSettingsRepository guildRepo, IRaidRepository raidRepo, ITierSettingsRepository tier, IRosterRepository roster)
    {
        _guildRepo = guildRepo;
        _raidRepo = raidRepo;
        _tierRepo = tier;
        _roster = roster;
    }
    
    public async Task SignUp(DiscordClient s, MessageReactionAddEventArgs e)
    {
        try
        {
            if (e.User.IsBot)
            {
                return;
            }
            var isUserQualified = false;
            string userRaidRole = "";
            var getRaid = await _raidRepo.GetStatus(e.Channel.Name);
            var getParent = await _guildRepo.CheckGuildSettings(e.Guild.Id);
            var getTiers = _tierRepo.GetAllTiers(e.Guild.Id);
            if (getRaid == null)
            {
                Console.WriteLine("Raid doesn't exist");
                return;
            }
            if (getParent?.RaidChannelGroup == null || e.Channel.Parent.Id != getParent.RaidChannelGroup)
            {
                Console.WriteLine("Issue signing up");
                return;
            }

            var getUserRoles = e.Guild.CurrentMember.Roles.ToList();
            if (getRaid.TierRole != null)
            {
                var getRoles = await _tierRepo.GetRoles(e.Guild.Id, getRaid.TierRole);
                
                for (int i = 0; i < getTiers?.Count; i++)
                {
                    Console.WriteLine(getTiers[i].TierName + " " + getRaid.TierRole);
                    if (getTiers[i].TierName != getRaid.TierRole) continue;
                    Console.WriteLine("Testing after if " + i);
                    for (int j = 0; j < getRoles.Count; j++)
                    {
                        Console.WriteLine("Testing inside second loop " + j);
                        foreach (var r in getUserRoles)
                        {
                            Console.WriteLine("Testing inside foreach " + getRoles[j].RoleName + " "  + r);
                            if (getRoles[j].RoleName?.ToLower() == r.Name.ToLower())
                            {
                                Console.WriteLine("Testing inside if " + r);
                                isUserQualified = true;
                                Console.WriteLine(isUserQualified);
                                break;
                            }
                        }
                    }
                }
            }

            if (isUserQualified)
            {
                var rosterManagement = new RaidRosterManagement(_roster);
                await s.SendMessageAsync(e.Channel,"Test");
                Console.WriteLine("Testing");
                rosterManagement?.AddMemberToRoster(e.Guild.CurrentMember);
            }
            else
            {
                Console.WriteLine("User isnt qualified");
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
        }
    }
}