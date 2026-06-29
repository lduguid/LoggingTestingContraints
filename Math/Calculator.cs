using LoggingTestingContraints.Contracts;
using Microsoft.Extensions.Logging;

namespace LoggingTestingContraints.Math;

public sealed class Calculator(IIntegerMath integerMath, ILogger<Calculator> logger) : ICalculator
{
    public int Add(int a, int b)
    {
        Contract.Require(true, "no preconditions");

        logger.LogDebug("Adding {A} and {B}", a, b);

        var result = a + b;

        Contract.Ensure(result == unchecked(a + b), "result must equal sum of operands");

        logger.LogInformation("Add completed: {A} + {B} = {Result}", a, b, result);
        return result;
    }

    public int Subtract(int a, int b)
    {
        Contract.Require(true, "no preconditions");

        logger.LogDebug("Subtracting {B} from {A}", b, a);

        var result = a - b;

        Contract.Ensure(result == unchecked(a - b), "result must equal difference of operands");

        logger.LogInformation("Subtract completed: {A} - {B} = {Result}", a, b, result);
        return result;
    }

    public int Multiply(int a, int b)
    {
        Contract.Require(true, "no preconditions");

        logger.LogDebug("Multiplying {A} and {B}", a, b);

        var result = a * b;

        Contract.Ensure(result == unchecked(a * b), "result must equal product of operands");

        logger.LogInformation("Multiply completed: {A} * {B} = {Result}", a, b, result);
        return result;
    }

    public int SafeDivide(int a, int b)
    {
        if (b == 0)
            throw new ArgumentOutOfRangeException(nameof(b), b, "divisor must not be zero");

        Contract.Require(b != 0, "divisor must not be zero");

        logger.LogDebug("Dividing {A} by {B}", a, b);

        var result = a / b;

        Contract.Ensure(result == a / b, "result must equal quotient of operands");

        logger.LogInformation("SafeDivide completed: {A} / {B} = {Result}", a, b, result);
        return result;
    }

    public int ComputeExpression(int a, int b, int c)
    {
        Contract.Require(true, "no preconditions");

        logger.LogDebug("Computing expression Abs(({A} + {B}) * {C})", a, b, c);

        var sum = Add(a, b);
        logger.LogDebug("Intermediate sum is {Sum}", sum);

        var product = Multiply(sum, c);
        logger.LogDebug("Intermediate product is {Product}", product);

        var result = integerMath.Abs(product);

        Contract.Ensure(result >= 0, "result must be non-negative");
        Contract.Ensure(result == integerMath.Abs(unchecked(a + b) * c), "result must equal Abs((a + b) * c)");

        logger.LogInformation(
            "ComputeExpression completed: Abs(({A} + {B}) * {C}) = {Result}",
            a,
            b,
            c,
            result);
        return result;
    }
}
