using LoggingTestingContraints.Logging;
using LoggingTestingContraints.Math;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggingBootstrap.CreateLoggerFactory();
var logger = loggerFactory.CreateLogger("Program");

logger.LogInformation("Application starting");

var sample = IntegerMath.Abs(-42, loggerFactory.CreateLogger("IntegerMath"));
logger.LogInformation("Sample absolute value computed: {Value}", sample);

logger.LogInformation("Application finished");
