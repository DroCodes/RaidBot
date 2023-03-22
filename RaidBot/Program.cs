using DSharpPlus;

public class Program
{
    static async Task Main(string[] args)
    {
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = "MTA1NTM0NzI2NzM1NzM4NDg2NA.GpYcf5.Kx-BloPTQnXwXmKeRdWT2VlAYiwghZ6i4Aed2Y",
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