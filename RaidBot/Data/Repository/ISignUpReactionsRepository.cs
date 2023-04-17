using DSharpPlus.Entities;
using RaidBot.entities;

namespace RaidBot.Data.Repository;

public interface ISignUpReactionsRepository
{
    Task<bool> AddSignUpReactions(ulong guildId, DiscordEmoji emoji, string raidRole);
    Task<List<SignUpEmoji>?> GetSignUpReactions(ulong guildId);
    Task<bool> DeleteSignUpReactions(ulong guildId, DiscordEmoji emoji);
}