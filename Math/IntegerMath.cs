using LoggingTestingContraints.Contracts;
using Microsoft.Extensions.Logging;

namespace LoggingTestingContraints.Math;

public sealed class IntegerMath(ILogger<IntegerMath> logger) : IIntegerMath
{
    public int Abs(int value)
    {
        Contract.Require(true, "no preconditions");

        logger.LogDebug("Computing absolute value for {Input}", value);

        var result = value < 0 ? -value : value;

        Contract.Ensure(result >= 0, "result must be non-negative");

        logger.LogDebug("Absolute value result is {Result}", result);
        return result;
    }
}
