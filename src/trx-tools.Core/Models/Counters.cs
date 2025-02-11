namespace trx_tools.Core.Models;

public record Counters(
    long Total,
    long Executed,
    long Passed,
    long Failed,
    long Error,
    long Timeout,
    long Aborted,
    long Inconclusive,
    long PassedButRunAborted,
    long NotRunnable,
    long NotExecuted,
    long Disconnected,
    long Warning,
    long Completed,
    long InProgress,
    long Pending
);