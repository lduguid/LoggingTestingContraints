using LoggingTestingContraints.Logging;
using LoggingTestingContraints.Math;
using Microsoft.Extensions.DependencyInjection;
using CalculatorImpl = LoggingTestingContraints.Math.Calculator;

namespace LoggingTestingContraints.Tests.Integration;

public class DiIntegrationTests
{
    [Fact]
    public void AppBootstrap_ResolvesCalculator_WithWiredDependencies()
    {
        using var services = AppBootstrap.CreateServices();

        var calculator = services.GetRequiredService<ICalculator>();
        var integerMath = services.GetRequiredService<IIntegerMath>();

        Assert.IsAssignableFrom<CalculatorImpl>(calculator);
        Assert.IsAssignableFrom<IntegerMath>(integerMath);

        Assert.Equal(42, calculator.Add(17, 25));
        Assert.Equal(20, calculator.ComputeExpression(2, 3, -4));
        Assert.Equal(5, calculator.SafeDivide(10, 2));
    }
}
