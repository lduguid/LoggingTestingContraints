using LoggingTestingContraints.Math;
using Microsoft.Extensions.Logging;

namespace LoggingTestingContraints.Demo;

public static class CalculatorScenario
{
    public const string ScenarioName = "CalculatorLearningDemo";

    public static void Run(ICalculator calculator, ILogger logger)
    {
        logger.LogInformation("Scenario {ScenarioName} starting", ScenarioName);

        RunStep(logger, "Add", () => calculator.Add(17, 25));
        RunStep(logger, "Subtract", () => calculator.Subtract(100, 42));
        RunStep(logger, "Multiply", () => calculator.Multiply(6, 7));
        RunStep(logger, "SafeDivide", () => calculator.SafeDivide(100, 4));
        RunStep(logger, "ComputeExpression", () => calculator.ComputeExpression(2, 3, -4));

        logger.LogInformation("Scenario {ScenarioName} completed", ScenarioName);
    }

    private static void RunStep(ILogger logger, string stepName, Func<int> operation)
    {
        var result = operation();
        logger.LogInformation(
            "Scenario {ScenarioName} step {Step} result {Result}",
            ScenarioName,
            stepName,
            result);
    }
}
