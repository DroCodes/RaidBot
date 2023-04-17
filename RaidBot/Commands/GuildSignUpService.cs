using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RaidBot.Data.Repository;
using RaidBot.Util;

namespace RaidBot.Commands;

public class GuildSignUpService : ApplicationCommandModule
{
    private readonly ISignUpReactionsRepository _repo;
    private readonly IMessageBuilder _msg;
    private string? _title;
    private string? _description;
    private DiscordColor _color;
    private const string? InitialResponse = "Thinking";

    public GuildSignUpService(ISignUpReactionsRepository repo, IMessageBuilder msg)
    {
        _repo = repo;
        _msg = msg;
    }

    [SlashCommand("SignUpEmoji", "The emoji used to sign up")]
    public async Task SaveSignUpEmojiCommand(InteractionContext ctx,
        [Option("Emoji", "The emoji to save")] string emoji,
        [Option("RaidRole", "Role to save (Tank, Dps, Healer)")]
        string raidRole)
    {
        ulong guildId = ctx.Guild.Id;
        DiscordEmoji? discordEmoji = null;

        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_msg.EmbedBuilder(InitialResponse)));

        try
        {
            if (emoji.StartsWith("<:") && emoji.EndsWith(">"))
            {
                string[] splitEmoji = emoji.TrimStart('<').TrimEnd('>').Split(':');
                if (splitEmoji.Length == 3 && ulong.TryParse(splitEmoji[2], out ulong emojiId))
                {
                    discordEmoji = DiscordEmoji.FromGuildEmote(ctx.Client, emojiId);
                }
            }
            else
            {
                // Check if the input is a Unicode emoji
                discordEmoji = DiscordEmoji.FromUnicode(emoji);
            }

            if (discordEmoji == null)
            {
                _title = "Error";
                _description = "Please enter a valid emoji";
                _color = DiscordColor.Red;
                
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }
            
            var addEmoji = await _repo.AddSignUpReactions(guildId, discordEmoji, raidRole);
            if (!addEmoji)
            {
                _title = "Error";
                _description = "Error saving emoji and role";
                _color = DiscordColor.Red;
            
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
                return;
            }

            _title = "Success";
            _description = "Successfully saved emoji and role";
            _color = DiscordColor.Green;

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [SlashCommand("DeleteEmoji", "Deletes the signup emoji")]
    public async Task DeleteEmojiCommand(InteractionContext ctx, [Option("Emoji", "The emoji to delete")] string emoji)
    {
        var guildId = ctx.Guild.Id;
        DiscordEmoji discordEmoji = null;

        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(_msg.EmbedBuilder(InitialResponse)));
        
        if (emoji.StartsWith("<:") && emoji.EndsWith(">"))
        {
            string[] splitEmoji = emoji.TrimStart('<').TrimEnd('>').Split(':');
            if (splitEmoji.Length == 3 && ulong.TryParse(splitEmoji[2], out ulong emojiId))
            {
                discordEmoji = DiscordEmoji.FromGuildEmote(ctx.Client, emojiId);
            }
        }
        else
        {
            // Check if the input is a Unicode emoji
            discordEmoji = DiscordEmoji.FromUnicode(emoji);
        }

        if (discordEmoji == null)
        {
            _title = "Error";
            _description = "Please enter a valid emoji";
            _color = DiscordColor.Red;
                
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }

        var deleteEmoji = await _repo.DeleteSignUpReactions(guildId, discordEmoji);

        if (!deleteEmoji)
        {
            _title = "Error";
            _description = "Error deleting emoji";
            _color = DiscordColor.Red;
                
            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
            return;
        }
        _title = "Success";
        _description = "Emoji Deleted";
        _color = DiscordColor.Green;
                
        await ctx.EditResponseAsync(
            new DiscordWebhookBuilder().AddEmbed(_msg.EmbedBuilder(_title, _description, _color)));
    }
}