# LoggingTestingContraints — Agent Instructions

Learning project for C# and .NET best practices. Every change should reinforce three core practices below.

## Project overview

- **Runtime**: .NET 9 console application
- **Tests**: xUnit project `LoggingTestingContraints.Tests`
- **Solution**: `LoggingTestingContraints.sln`

## Build and test commands

```bash
dotnet build
dotnet test
dotnet run --project LoggingTestingContraints
```

Run tests before considering any task complete. Prefer focused runs during development:

```bash
dotnet test --filter "FullyQualifiedName~ClassName"
```

---

## Core practice 1: Structured logging from day one

Use `Microsoft.Extensions.Logging` (via `ILogger<T>`) — never `Console.WriteLine` for application logic.

- **Inject** `ILogger<T>` through the constructor; do not pass loggers as method parameters.
- Register services in `Logging/AppBootstrap.cs` via `Microsoft.Extensions.DependencyInjection`.
- Resolve dependencies at the composition root (`Program.cs`) with `GetRequiredService<T>()`.
- Use structured placeholders: `logger.LogInformation("Processed {Count} items", count)` — not string interpolation.
- Log at appropriate levels: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`.
- Serilog writes JSON logs to `logs/` so output can be replayed, diffed, and used for regression analysis.
- When investigating bugs or regressions, compare log files across runs before changing behavior.

**Do not** remove or downgrade existing log statements without explicit reason.

### DI pattern

```csharp
// AppBootstrap.cs — register once
services.AddLogging(builder => builder.AddSerilog(dispose: true));
services.AddSingleton<IMyService, MyService>();

// MyService.cs — logger injected at construction
public sealed class MyService(ILogger<MyService> logger) : IMyService { ... }

// Program.cs — resolve at entry point
var myService = services.GetRequiredService<IMyService>();
```

In unit tests, construct the class directly with `NullLogger<T>.Instance` — no container needed for isolated tests.

Use `LoggingTestingContraints.Tests/Integration/DiIntegrationTests.cs` to verify `AppBootstrap` wiring end-to-end.

---

## Core practice 2: Function contracts (pre/post conditions)

Every non-trivial function must document and enforce its contract.

### Requirements

1. **Document** preconditions and postconditions in XML doc comments on the method.
2. **Enforce** with `Contract.Require(...)` at entry (preconditions) and `Contract.Ensure(...)` before return (postconditions).
3. Contracts live in `Contracts/Contract.cs` and use `Debug.Assert` in DEBUG builds — they catch integrity drift during development and testing.
4. Keep contracts **minimal and truthful**: only assert what the function actually guarantees.

### Example

```csharp
public sealed class IntegerMath(ILogger<IntegerMath> logger) : IIntegerMath
{
    /// <summary>Computes the absolute value of an integer.</summary>
    /// <precondition>none</precondition>
    /// <postcondition>result >= 0</postcondition>
    public int Abs(int value)
    {
        Contract.Require(true, "no preconditions");
        var result = value < 0 ? -value : value;
        Contract.Ensure(result >= 0, "result must be non-negative");
        return result;
    }
}
```

### Worked example: runtime guard + contract (`SafeDivide`)

When a precondition can be violated by callers, enforce it at runtime **first**, then assert the contract in DEBUG on the path that continues:

```csharp
public int SafeDivide(int a, int b)
{
    // Runtime enforcement — all builds; unit tests assert this
    if (b == 0)
        throw new ArgumentOutOfRangeException(nameof(b), b, "divisor must not be zero");

    // DEBUG-only contract echo (after guard; documents valid-path invariant)
    Contract.Require(b != 0, "divisor must not be zero");

    var result = a / b;

    Contract.Ensure(result == a / b, "result must equal quotient of operands");
    return result;
}
```

**Why guard before `Require`?** `Contract.Require` uses `Debug.Assert`, which aborts in DEBUG before your `throw` runs. Tests and Release builds must see the same `ArgumentOutOfRangeException`, not `DebugAssertException`.

### Breaking a contract

If a code change would violate an existing pre/post condition:

1. **Stop** — do not silently weaken or delete assertions.
2. **Verify** the change is required: explain why the old contract is wrong or incomplete.
3. **Update** the XML contract documentation, assertions, and **all affected unit tests** together in the same change.
4. **Record** the rationale in the commit message or PR description.

---

## Core practice 3: Test-driven development (TDD)

Write tests first whenever adding or changing behavior.

### Workflow (Red → Green → Refactor)

1. **Red**: Add a failing unit test that describes the desired behavior and documents expected pre/post conditions where relevant.
2. **Green**: Implement the minimum code to pass the test, including `Contract.Require` / `Contract.Ensure`.
3. **Refactor**: Clean up while keeping tests green; do not remove contract checks.

### Test conventions

- Test project: `LoggingTestingContraints.Tests` (xUnit)
- Mirror production namespaces: `LoggingTestingContraints.Tests/<Area>/<Class>Tests.cs`
- Name tests: `MethodName_State_ExpectedResult` (e.g. `Abs_NegativeInput_ReturnsPositive`)
- Use `[Fact]` for single cases, `[Theory]` + `[InlineData]` for parameterized cases
- Test public behavior and contract boundaries — including invalid inputs that should throw or fail assertions

### Agent behavior

When asked to implement a feature:

1. Propose or write the test(s) first.
2. Confirm tests fail (or would fail) without implementation.
3. Then implement production code with logging and contracts.
4. Run `dotnet test` and report results.

---

## Code style

- Enable nullable reference types; handle null explicitly.
- Prefer small, single-purpose functions.
- Match existing naming and folder layout.
- Keep changes focused; avoid unrelated refactors.

## Logging output location

- JSON logs: `logs/log-yyyyMMdd-HHmmss.json` (one file per run)
- Console: human-readable during development
- `logs/` is gitignored — do not commit log files

### Log replay / regression

`Demo/CalculatorScenario.cs` runs a fixed scripted scenario (`CalculatorLearningDemo`) with structured `{ScenarioName}`, `{Step}`, and `{Result}` properties in each Information log entry.

Scripts in `scripts/` (run from project root):

```powershell
dotnet run

# list recent log files
.\scripts\List-Logs.ps1

# compare two specific runs (behavioral — timestamps ignored)
.\scripts\Compare-Logs.ps1 logs\log-20260628-195903.json logs\log-20260628-195934.json

# compare the two most recent runs
.\scripts\Compare-Logs.ps1 -Latest

# include timestamp differences
.\scripts\Compare-Logs.ps1 -Latest -Raw
```

Identical inputs should produce identical JSON log lines (except timestamps). Diff logs before changing behavior when investigating regressions. `Compare-Logs.ps1` exits `0` when logs match, `1` when they differ.
