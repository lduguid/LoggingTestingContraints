using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace LoggingTestingContraints.Logging;

public static class LoggingBootstrap
{
    public static ILoggerFactory CreateLoggerFactory()
    {
        Directory.CreateDirectory("logs");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                new CompactJsonFormatter(),
                path: "logs/log-.json",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        return LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(dispose: true);
        });
    }
}
