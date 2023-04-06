using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RaidBot.Commands;
using RaidBot.Commands.RaidCommands;
using RaidBot.Data;
using RaidBot.Data.Repository;
using RaidBot.Util;
using Serilog;
using ILogger = RaidBot.Util.ILogger;

public class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .Build();
        
        var connectionString = builder.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .MinimumLevel.Debug()
               .CreateLogger();

        var logFactory = new LoggerFactory().AddSerilog();

        string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");


        // Create a new Discord client
        var client = new DiscordClient(new DiscordConfiguration()
        {
            Token = token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = logFactory
        });

        var services = new ServiceCollection()
            .AddSingleton<IGuildSettingsRepository, GuildSettingsRepository>()
            .AddSingleton<IRaidSettingsRepository, RaidSettingsRepository>()
            .AddSingleton<ITierSettingsRepository, TierSettingsRepository>()
            .AddSingleton<IMessageBuilder, MessageBuilder>()
            .AddSingleton<ILogger, Logger>()
            .AddDbContext<DataContext>(options => options.UseNpgsql(connectionString))
            .BuildServiceProvider();

        var slashCommands = client.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services
        });

        slashCommands.RegisterCommands<TestCommands>();
        slashCommands.RegisterCommands<GuildSettingsCommands>();
        slashCommands.RegisterCommands<RaidCreationCommands>();
        slashCommands.RegisterCommands<TierSettingsCommands>();
        slashCommands.RegisterCommands<RaidSettingsCommands>();

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
                    services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
                });
}