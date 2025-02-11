namespace trx_tools.Core.Models;

public record TestRun(
    Times Times,
    TestSettings TestSettings,
    Results Results,
    TestDefinitions TestDefinitions,
    TestEntries TestEntries,
    TestLists TestLists,
    ResultSummary ResultSummary,
    Guid Id,
    string Name,
    RunUser RunUser
);