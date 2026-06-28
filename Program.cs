using LoggingTestingContraints.Logging;
using LoggingTestingContraints.Math;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

await using var services = AppBootstrap.CreateServices();

var logger = services.GetRequiredService<ILogger<Program>>();
var integerMath = services.GetRequiredService<IIntegerMath>();

logger.LogInformation("Application starting");

var sample = integerMath.Abs(-42);
logger.LogInformation("Sample absolute value computed: {Value}", sample);

logger.LogInformation("Application finished");
