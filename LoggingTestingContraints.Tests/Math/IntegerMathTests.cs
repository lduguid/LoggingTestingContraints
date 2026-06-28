using LoggingTestingContraints.Math;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoggingTestingContraints.Tests.Math;

public class IntegerMathTests
{
    [Theory]
    [InlineData(5, 5)]
    [InlineData(-5, 5)]
    [InlineData(0, 0)]
    public void Abs_Input_ReturnsAbsoluteValue(int input, int expected)
    {
        var result = IntegerMath.Abs(input, NullLogger.Instance);

        Assert.Equal(expected, result);
        Assert.True(result >= 0);
    }
}
