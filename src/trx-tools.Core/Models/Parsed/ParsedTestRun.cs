namespace trx_tools.Core.Models.Parsed;

public record ParsedTestRun(
    Times? Times,
    List<ParsedUnitTestResult> Results,
    ResultSummary.ResultSummary ResultSummary,
    string Id,
    string Name,
    string RunUser
);