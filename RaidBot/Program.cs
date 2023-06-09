﻿using DSharpPlus;
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
using RaidBot.Events;
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
        
        var context = new DataContext(optionsBuilder.Options);
        var logger = new Logger();

        var guildSettings = new GuildSettingsRepository(context, logger);
        var raidSettings = new RaidRepository(context, logger);
        var tiers = new TierSettingsRepository(context, logger);
        var roster = new RosterRepository(context, logger);
        
        var reactionSignup = new ReactionSignUpEvent(guildSettings, raidSettings, tiers, roster);
        
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

        client.MessageReactionAdded += reactionSignup.SignUp;

        var services = new ServiceCollection()
            .AddSingleton<IGuildSettingsRepository, GuildSettingsRepository>()
            .AddSingleton<ITierSettingsRepository, TierSettingsRepository>()
            .AddSingleton<IMessageBuilder, MessageBuilder>()
            .AddSingleton<ILogger, Logger>()
            .AddSingleton<IRaidRepository, RaidRepository>()
            .AddSingleton<IRaidInfoRepository, RaidInfoRepository>()
            .AddSingleton<IRaidRolesRepository, RaidRolesRepository>()
            .AddDbContext<DataContext>(options => options.UseNpgsql(connectionString))
            .BuildServiceProvider();

        var slashCommands = client.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services
        });
        
        slashCommands.RegisterCommands<GuildSettingsCommands>();
        slashCommands.RegisterCommands<RaidManagementService>();
        slashCommands.RegisterCommands<TierSettingsCommands>();
        slashCommands.RegisterCommands<RaidListService>();
        slashCommands.RegisterCommands<RaidRoleService>();
        slashCommands.RegisterCommands<RaidStatusService>();
        slashCommands.RegisterCommands<GuildSignUpService>();

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