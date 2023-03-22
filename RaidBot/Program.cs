using DSharpPlus;

public class Program
{
    static async Task Main(string[] args)
    {
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All
        });

        client.MessageCreated += async (s, e) =>
        {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("pong!");
        };

        await client.ConnectAsync();
        await Task.Delay(-1);
    }
}