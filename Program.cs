using LoggingTestingContraints.Demo;
using LoggingTestingContraints.Logging;
using LoggingTestingContraints.Math;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

await using var services = AppBootstrap.CreateServices();

var logger = services.GetRequiredService<ILogger<Program>>();
var calculator = services.GetRequiredService<ICalculator>();

logger.LogInformation("Application starting");

CalculatorScenario.Run(calculator, logger);

logger.LogInformation("Application finished");
