using LoggingTestingContraints.Math;
using Microsoft.Extensions.Logging.Abstractions;
using CalculatorImpl = LoggingTestingContraints.Math.Calculator;

namespace LoggingTestingContraints.Tests.Calculator;

public class CalculatorTests
{
    private static ICalculator CreateSut() =>
        new CalculatorImpl(
            new IntegerMath(NullLogger<IntegerMath>.Instance),
            NullLogger<CalculatorImpl>.Instance);

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-2, 3, 1)]
    [InlineData(-2, -3, -5)]
    public void Add_TwoNumbers_ReturnsSum(int a, int b, int expected)
    {
        var result = CreateSut().Add(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 0, 5)]
    [InlineData(0, 5, 5)]
    [InlineData(0, 0, 0)]
    public void Add_WithZero_ReturnsOtherOperand(int a, int b, int expected)
    {
        var result = CreateSut().Add(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Add_MaxValuePlusOne_WrapsToMinValue()
    {
        var result = CreateSut().Add(int.MaxValue, 1);

        Assert.Equal(int.MinValue, result);
    }

    [Theory]
    [InlineData(int.MaxValue, 1, int.MinValue)]
    [InlineData(int.MaxValue, int.MaxValue, -2)]
    [InlineData(int.MinValue, -1, int.MaxValue)]
    [InlineData(int.MinValue, int.MinValue, 0)]
    public void Add_OverflowBoundary_ThrowsUnderChecked_ReturnsUncheckedWrap(int a, int b, int expectedUnchecked)
    {
        Assert.Throws<OverflowException>(() => checked(a + b));

        var result = CreateSut().Add(a, b);

        Assert.Equal(expectedUnchecked, result);
        Assert.Equal(unchecked(a + b), result);
    }

    [Theory]
    [InlineData(5, 3, 2)]
    [InlineData(-2, 3, -5)]
    [InlineData(3, 5, -2)]
    public void Subtract_TwoNumbers_ReturnsDifference(int a, int b, int expected)
    {
        var result = CreateSut().Subtract(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 0, 5)]
    [InlineData(-3, 0, -3)]
    [InlineData(0, 0, 0)]
    public void Subtract_WithZero_ReturnsMinuend(int a, int b, int expected)
    {
        var result = CreateSut().Subtract(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(int.MinValue, 1, int.MaxValue)]
    [InlineData(int.MaxValue, -1, int.MinValue)]
    [InlineData(int.MinValue, int.MaxValue, 1)]
    public void Subtract_OverflowBoundary_ThrowsUnderChecked_ReturnsUncheckedWrap(int a, int b, int expectedUnchecked)
    {
        Assert.Throws<OverflowException>(() => checked(a - b));

        var result = CreateSut().Subtract(a, b);

        Assert.Equal(expectedUnchecked, result);
        Assert.Equal(unchecked(a - b), result);
    }

    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(-2, 3, -6)]
    [InlineData(-2, -3, 6)]
    public void Multiply_TwoNumbers_ReturnsProduct(int a, int b, int expected)
    {
        var result = CreateSut().Multiply(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0, 5, 0)]
    [InlineData(5, 0, 0)]
    [InlineData(0, 0, 0)]
    public void Multiply_ByZero_ReturnsZero(int a, int b, int expected)
    {
        var result = CreateSut().Multiply(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(int.MaxValue, 2, -2)]
    [InlineData(int.MinValue, -1, int.MinValue)]
    public void Multiply_OverflowBoundary_ThrowsUnderChecked_ReturnsUncheckedWrap(int a, int b, int expectedUnchecked)
    {
        Assert.Throws<OverflowException>(() => checked(a * b));

        var result = CreateSut().Multiply(a, b);

        Assert.Equal(expectedUnchecked, result);
        Assert.Equal(unchecked(a * b), result);
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(7, 3, 2)]
    [InlineData(0, 5, 0)]
    public void SafeDivide_ValidInputs_ReturnsQuotient(int a, int b, int expected)
    {
        var result = CreateSut().SafeDivide(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 0)]
    [InlineData(-5, 0)]
    [InlineData(0, 0)]
    public void SafeDivide_DivisorZero_ThrowsArgumentOutOfRangeException(int a, int b)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => CreateSut().SafeDivide(a, b));

        Assert.Equal("b", ex.ParamName);
    }

    [Theory]
    [InlineData(-7, 2, -3)]
    [InlineData(7, -2, -3)]
    [InlineData(-7, -2, 3)]
    [InlineData(5, 2, 2)]
    public void SafeDivide_NegativeOperands_ReturnsTruncatedQuotient(int a, int b, int expected)
    {
        var result = CreateSut().SafeDivide(a, b);

        Assert.Equal(expected, result);
        Assert.Equal(a / b, result);
    }

    [Theory]
    [InlineData(2, 3, 4, 20)]
    [InlineData(1, 1, 5, 10)]
    [InlineData(0, 0, 100, 0)]
    public void ComputeExpression_PositiveOrZeroResult_ReturnsExpected(int a, int b, int c, int expected)
    {
        var result = CreateSut().ComputeExpression(a, b, c);

        Assert.Equal(expected, result);
        Assert.True(result >= 0);
    }

    [Theory]
    [InlineData(1, 2, -3, 9)]
    [InlineData(-2, -1, -2, 6)]
    [InlineData(5, -10, 2, 10)]
    public void ComputeExpression_NegativeIntermediateProduct_ReturnsAbsoluteValue(int a, int b, int c, int expected)
    {
        var result = CreateSut().ComputeExpression(a, b, c);

        Assert.Equal(expected, result);
        Assert.True(result >= 0);
    }
}
