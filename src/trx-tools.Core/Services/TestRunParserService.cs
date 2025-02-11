using Microsoft.Extensions.Logging;
using trx_tools.Core.Exceptions;
using trx_tools.Core.Models;
using trx_tools.Core.Models.Parsed;
using trx_tools.Core.Services.Interfaces;

namespace trx_tools.Core.Services;

public class TestRunParserService : ITestRunParserService
{
    private readonly ILogger<TestRunParserService> _logger;

    public TestRunParserService(ILogger<TestRunParserService> logger)
    {
        _logger = logger;
    }

    public ParsedTestRun ParseTestRun(TestRun testRun)
    {
        ArgumentNullException.ThrowIfNull(testRun, nameof(testRun));
        
        _logger.LogInformation("Parsing test run");
        var parsedResults = new List<ParsedUnitTestResult>();
        foreach (var testEntry in testRun.TestEntries)
        {
            var unitTest = testRun.TestDefinitions.FirstOrDefault(x => x.Id == testEntry.TestId);
            if (unitTest is null)
            {
                throw new UnitTestDataNotFoundException($"Could not find test definition for test entry with ID {testEntry.TestId}");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("unitTest found for test entry with ID {TestId}", testEntry.TestId);
            }

            var testResult = testRun.Results.FirstOrDefault(x => x.ExecutionId == testEntry.ExecutionId);
            if (testResult is null)
            {
                throw new TestResultDataNotFoundException($"Could not find test result for test entry with execution ID {testEntry.ExecutionId}");
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("testResult found for test entry with execution ID {ExecutionId}", testEntry.ExecutionId);
            }

            if (testResult.TestId != testEntry.TestId)
            {
                throw new TestIdsMismatchException($"Test ID mismatch between test entry and test result. Test entry ID: {testEntry.TestId}, Test result ID: {testResult.TestId}");
            }

            parsedResults.Add(new ParsedUnitTestResult(
                testResult.Outcome == "Passed",
                testResult.Outcome,
                unitTest.TestMethod.ClassName,
                unitTest.TestMethod.Name,
                TimeSpan.Parse(testResult.Duration)
            ));
        }
        
        _logger.LogInformation("Parsed {Count} test results", parsedResults.Count);
        return new ParsedTestRun(
            testRun.Times,
            parsedResults,
            testRun.ResultSummary,
            testRun.Id,
            testRun.Name,
            testRun.RunUser
        );
    }
}