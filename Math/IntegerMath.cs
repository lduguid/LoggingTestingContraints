using LoggingTestingContraints.Contracts;
using Microsoft.Extensions.Logging;

namespace LoggingTestingContraints.Math;

public static class IntegerMath
{
    /// <summary>Returns the absolute value of an integer.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result >= 0</postcondition>
    public static int Abs(int value, ILogger? logger = null)
    {
        Contract.Require(true, "no preconditions");

        logger?.LogDebug("Computing absolute value for {Input}", value);

        var result = value < 0 ? -value : value;

        Contract.Ensure(result >= 0, "result must be non-negative");

        logger?.LogDebug("Absolute value result is {Result}", result);
        return result;
    }
}
