namespace LoggingTestingContraints.Math;

public interface ICalculator
{
    /// <summary>Returns the sum of two integers using unchecked arithmetic.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result == a + b (wraps on overflow)</postcondition>
    int Add(int a, int b);

    /// <summary>Returns the difference of two integers using unchecked arithmetic.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result == a - b (wraps on overflow)</postcondition>
    int Subtract(int a, int b);

    /// <summary>Returns the product of two integers using unchecked arithmetic.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result == a * b (wraps on overflow)</postcondition>
    int Multiply(int a, int b);

    /// <summary>Returns the integer quotient of two integers (truncates toward zero).</summary>
    /// <precondition>b != 0</precondition>
    /// <postcondition>result == a / b</postcondition>
    int SafeDivide(int a, int b);

    /// <summary>Evaluates Abs((a + b) * c) using composed calculator and integer math operations.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result == absolute value of ((a + b) * c)</postcondition>
    int ComputeExpression(int a, int b, int c);
}
