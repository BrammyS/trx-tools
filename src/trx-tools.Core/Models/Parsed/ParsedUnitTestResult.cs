using trx_tools.Core.Models.Results;

namespace trx_tools.Core.Models.Parsed;

public record ParsedUnitTestResult(
    bool IsSuccess,
    string? Outcome,
    string Class,
    string Name,
    string Codebase,
    string Category,
    TimeSpan Duration,
    UnitTestResultOutput Output
)
{
    public bool IsFailed => !IsSuccess && Outcome != Constants.NotExecutedTestRunOutcome;
};