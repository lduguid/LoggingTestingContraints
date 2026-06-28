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

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                new CompactJsonFormatter(),
                path: "logs/log-.json",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var services = new ServiceCollection();

        services.AddLogging(builder => builder.AddSerilog(dispose: true));
        services.AddSingleton<IIntegerMath, IntegerMath>();

        return services.BuildServiceProvider();
    }
}
