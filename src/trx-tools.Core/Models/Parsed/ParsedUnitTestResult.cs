using trx_tools.Core.Models.Results;

namespace trx_tools.Core.Models.Parsed;

public record ParsedUnitTestResult(
    bool IsSuccess,
    string? Outcome,
    string Class,
    string Name,
    TimeSpan Duration,
    UnitTestResultOutput Output
)
{
    public string FullName => $"{Class}.{Name}";
}