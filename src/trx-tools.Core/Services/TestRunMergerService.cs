using Microsoft.Extensions.Logging;
using trx_tools.Core.Exceptions;
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
        ArgumentNullException.ThrowIfNull(testRuns);
        logger.LogInformation("Merging {Count} test runs", testRuns.Count);
        
        var mergedUnitTestResults = new List<UnitTestResult>();
        var mergedTestEntries = new List<TestEntry>();
        var mergedUnitTests = new List<UnitTest>();
        var mergedTestLists = new List<TestList>();
        foreach (var testRun in testRuns.Where(x => x.ResultSummary.Counters.Total > 0))
        {
            foreach (var testEntry in testRun.TestEntries)
            {
                if (mergedTestEntries.Any(x => x.ExecutionId == testEntry.ExecutionId))
                {
                    throw new DuplicatedTestExecutionFoundException($"Duplicated test execution found with ID {testEntry.ExecutionId}");
                }
            }
            
            mergedUnitTestResults.AddRange(testRun.Results);
            mergedUnitTests.AddRange(testRun.TestDefinitions);
            mergedTestEntries.AddRange(testRun.TestEntries);
            mergedTestLists.AddRange(testRun.TestLists);
        }
        
        var counters = new Counters();
        foreach (var summary in testRuns.Select(testRun => testRun.ResultSummary.Counters))
        {
            counters.Total += summary.Total;
            counters.Executed += summary.Executed;
            counters.Passed += summary.Passed;
            counters.Failed += summary.Failed;
            counters.Error += summary.Error;
            counters.Timeout += summary.Timeout;
            counters.Aborted += summary.Aborted;
            counters.Inconclusive += summary.Inconclusive;
            counters.PassedButRunAborted += summary.PassedButRunAborted;
            counters.NotRunnable += summary.NotRunnable;
            counters.NotExecuted += summary.NotExecuted;
            counters.Disconnected += summary.Disconnected;
            counters.Warning += summary.Warning;
            counters.Completed += summary.Completed;
            counters.InProgress += summary.InProgress;
            counters.Pending += summary.Pending;
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
                Counters = counters,
                Output = new Output
                {
                    StdOut = string.Join(Environment.NewLine, testRuns.Where(x => x.ResultSummary.Output is not null).Select(x => x.ResultSummary.Output!.StdOut))
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