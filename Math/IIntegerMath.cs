namespace LoggingTestingContraints.Math;

public interface IIntegerMath
{
    /// <summary>Returns the absolute value of an integer.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result >= 0</postcondition>
    int Abs(int value);
}
