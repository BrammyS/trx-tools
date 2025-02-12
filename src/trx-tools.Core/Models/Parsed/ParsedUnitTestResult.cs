namespace trx_tools.Core.Models.Parsed;

public record ParsedUnitTestResult(
    bool IsSuccess,
    string? Outcome,
    string Class,
    string Name,
    TimeSpan Duration
)
{
    public string FullName => $"{Class}.{Name}";
}