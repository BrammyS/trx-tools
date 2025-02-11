namespace trx_tools.Core.Models;

public record UnitTest(
    Execution Execution,
    TestMethod TestMethod,
    string Name,
    string Storage,
    Guid Id
);