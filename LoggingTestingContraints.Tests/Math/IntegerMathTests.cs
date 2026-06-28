using LoggingTestingContraints.Math;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoggingTestingContraints.Tests.Math;

public class IntegerMathTests
{
  private static IIntegerMath CreateSut() =>
      new IntegerMath(NullLogger<IntegerMath>.Instance);

  [Theory]
  [InlineData(5, 5)]
  [InlineData(-5, 5)]
  [InlineData(0, 0)]
  public void Abs_Input_ReturnsAbsoluteValue(int input, int expected)
  {
    var result = CreateSut().Abs(input);

    Assert.Equal(expected, result);
    Assert.True(result >= 0);
  }
}
