using Serilog;

public class Logger : RaidBot.Util.ILogger
{
    private readonly ILogger _logger;

    public Logger()
    {
        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogError(Exception ex, string message)
    {
        _logger.Error(ex, message);
    }
}

