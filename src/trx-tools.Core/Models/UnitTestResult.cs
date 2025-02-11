namespace trx_tools.Core.Models;

public record UnitTestResult(
    Guid ExecutionId,
    Guid TestId,
    string TestName,
    RunUser ComputerName,
    DateTimeOffset Duration,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    Guid TestType,
    string Outcome,
    Guid TestListId,
    Guid RelativeResultsDirectory
);