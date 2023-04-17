using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class SignUpReactionsRepository : ISignUpReactionsRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    public SignUpReactionsRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }

    public async Task<bool> AddSignUpReactions(ulong guildId, DiscordEmoji emoji, string raidRole)
    {
        try
        {
            var findEmoji = await _context.SignUpEmojis.FirstOrDefaultAsync(x => x.GuildId == guildId && x.EmojiName == emoji.Name);

            if (findEmoji != null)
            {
                return false;
            }

            var findGuild = await _context.GuildSettings.Include(g => g.Emoji)
                .FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (findGuild == null)
            {
                return false;
            }

            var newSignUpEmoji = new SignUpEmoji()
            {
                GuildId = guildId,
                RaidRole = raidRole,
                EmojiName = emoji.Name,
                GuildSettings = findGuild
            };

            Console.WriteLine(newSignUpEmoji.EmojiName);

            findGuild.Emoji.Add(newSignUpEmoji);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There was an error saving the emoji");
            return false;
        }
    }

    public async Task<List<SignUpEmoji>?> GetSignUpReactions(ulong guildId)
    {
        try
        {
            var findGuild = await _context.GuildSettings.Include(x => x.Emoji).FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (findGuild == null)
            {
                return null;
            }

            var emojiList = new List<SignUpEmoji>();

            foreach (var emoji in findGuild.Emoji)
            {
                emojiList.Add(emoji);
            }

            return emojiList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving the emoji");
            return null;
        }
    }


    public async Task<bool> DeleteSignUpReactions(ulong guildId, DiscordEmoji emoji)
    {
        try
        {
            var findEmoji =
                await _context.SignUpEmojis.FirstOrDefaultAsync(x => x.EmojiName == emoji.Name && x.GuildId == guildId);
            if (findEmoji == null)
            {
                return false;
            }

            var findGuild = await _context.GuildSettings.Include(x => x.Emoji).FirstOrDefaultAsync(x => x.GuildId == guildId);

            if (findGuild == null)
            {
                return false;
            }

            _context.SignUpEmojis.Remove(findEmoji);
            findGuild.Emoji.Remove(findEmoji);

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}