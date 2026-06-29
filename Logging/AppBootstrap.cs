using LoggingTestingContraints.Math;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace LoggingTestingContraints.Logging;

public static class AppBootstrap
{
    public static ServiceProvider CreateServices()
    {
        Directory.CreateDirectory("logs");

        var logPath = $"logs/log-{DateTime.Now:yyyyMMdd-HHmmss}.json";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(new CompactJsonFormatter(), path: logPath)
            .CreateLogger();

        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddSerilog(dispose: true));
        services.AddSingleton<IIntegerMath, IntegerMath>();
        services.AddSingleton<ICalculator, Calculator>();

        return services.BuildServiceProvider();
    }
}
