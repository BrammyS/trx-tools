using Microsoft.Extensions.Logging;
using trx_tools.Core.Models;
using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.ResultSummary;
using trx_tools.Core.Models.TestDefinitions;
using trx_tools.Core.Models.TestEntries;
using trx_tools.Core.Models.TestLists;
using trx_tools.Core.Services.Interfaces;

namespace trx_tools.Core.Services;

public class TestRunMergerService(ILogger<TestRunMergerService> logger) : ITestRunMergerService
{
    public TestRun MergeTestRuns(List<TestRun> testRuns)
    {
        ArgumentNullException.ThrowIfNull(testRuns, nameof(testRuns));
        logger.LogInformation("Merging {Count} test runs", testRuns.Count);
        
        var mergedUnitTestResults = new List<UnitTestResult>();
        var mergedTestEntries = new List<TestEntry>();
        var mergedUnitTests = new List<UnitTest>();
        var mergedTestLists = new List<TestList>();
        foreach (var testRun in testRuns)
        {
            mergedUnitTestResults.AddRange(testRun.Results);
            mergedUnitTests.AddRange(testRun.TestDefinitions);
            mergedTestEntries.AddRange(testRun.TestEntries);
            mergedTestLists.AddRange(testRun.TestLists);
        }
        
        return new TestRun
        {
            Times = null,
            TestSettings = null,
            Results = mergedUnitTestResults.ToArray(),
            TestDefinitions = mergedUnitTests.ToArray(),
            TestEntries = mergedTestEntries.ToArray(),
            TestLists = mergedTestLists.ToArray(),
            ResultSummary = new ResultSummary
            {
                Counters = new Counters
                {
                    Total = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Total),
                    Executed = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Executed),
                    Passed = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Passed),
                    Failed = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Failed),
                    Error = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Error),
                    Timeout = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Timeout),
                    Aborted = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Aborted),
                    Inconclusive = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Inconclusive),
                    PassedButRunAborted = (uint)testRuns.Sum(x => x.ResultSummary.Counters.PassedButRunAborted),
                    NotRunnable = (uint)testRuns.Sum(x => x.ResultSummary.Counters.NotRunnable),
                    NotExecuted = (uint)testRuns.Sum(x => x.ResultSummary.Counters.NotExecuted),
                    Disconnected = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Disconnected),
                    Warning = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Warning),
                    Completed = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Completed),
                    InProgress = (uint)testRuns.Sum(x => x.ResultSummary.Counters.InProgress),
                    Pending = (uint)testRuns.Sum(x => x.ResultSummary.Counters.Pending)
                },
                Output = new Output
                {
                    StdOut = string.Join(Environment.NewLine, testRuns.Select(x => x.ResultSummary.Output.StdOut))
                },
                Outcome = testRuns.All(x => x.ResultSummary.Outcome == "Passed") ? "Passed" : "Failed",
                RunInfos = []
            },
            Id = testRuns.First().Id,
            Name = testRuns.First().Name,
            RunUser = testRuns.First().RunUser
        };
    }
}