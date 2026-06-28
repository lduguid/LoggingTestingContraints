using System.Diagnostics;

namespace LoggingTestingContraints.Contracts;

/// <summary>
/// Design-by-contract helpers for precondition (Require) and postcondition (Ensure) checks.
/// Assertions run in DEBUG builds only; use unit tests to verify behavior in all configurations.
/// </summary>
public static class Contract
{
    [Conditional("DEBUG")]
    public static void Require(bool condition, string message) =>
        Debug.Assert(condition, message);

    [Conditional("DEBUG")]
    public static void Ensure(bool condition, string message) =>
        Debug.Assert(condition, message);
}
