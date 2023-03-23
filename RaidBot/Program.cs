using DSharpPlus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RaidBot.Data;
using Serilog;

public class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration builder;
        builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
           .Build();

        var connectionString = builder.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlite(connectionString);

        Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .MinimumLevel.Debug()
               .CreateLogger();

        var logFactory = new LoggerFactory().AddSerilog();

        // Create a new Discord client
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = logFactory
        });
        // use this to check bot is recieving messages
        client.MessageCreated += async (s, e) =>
        {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("pong!");
        };
        // Connect to the gateway
        await client.ConnectAsync();
        await Task.Delay(-1);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration builder;
                    builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                        .Build();

                    services.AddSingleton<IConfiguration>(builder);

                    var connectionString = builder.GetConnectionString("DefaultConnection");
                    services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
                });
}