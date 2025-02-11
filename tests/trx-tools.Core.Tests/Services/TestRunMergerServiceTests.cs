using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.Core.Models;
using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.ResultSummary;
using trx_tools.Core.Models.TestDefinitions;
using trx_tools.Core.Models.TestEntries;
using trx_tools.Core.Models.TestLists;
using trx_tools.Core.Services;

namespace trx_tools.Core.Tests.Services;

[TestFixture]
public class TestRunMergerServiceTests
{
    private readonly TestRun _failedTestRun = new()
    {
        Results =
        [
            new UnitTestResult
            {
                Outcome = "Passed",
                Output = null!,
                ExecutionId = null!,
                TestId = null!,
                TestName = null!,
                ComputerName = null!,
                Duration = null!,
                TestType = null!,
                TestListId = null!,
                RelativeResultsDirectory = null!
            },
            new UnitTestResult
            {
                Outcome = "Failed",
                Output = null!,
                ExecutionId = null!,
                TestId = null!,
                TestName = null!,
                ComputerName = null!,
                Duration = null!,
                TestType = null!,
                TestListId = null!,
                RelativeResultsDirectory = null!
            }
        ],
        TestDefinitions =
        [
            new UnitTest
            {
                TestMethod = new TestMethod
                {
                    ClassName = "Class1",
                    Name = "Method1",
                    CodeBase = null!,
                    AdapterTypeName = null!
                },
                Execution = null!,
                Name = null!,
                Storage = null!,
                Id = null!
            },
            new UnitTest
            {
                TestMethod = new TestMethod
                {
                    ClassName = "Class2",
                    Name = "Method2",
                    CodeBase = null!,
                    AdapterTypeName = null!
                },
                Execution = null!,
                Name = null!,
                Storage = null!,
                Id = null!
            }
        ],
        TestEntries =
        [
            new TestEntry
            {
                TestId = "1",
                ExecutionId = "1",
                TestListId = null!
            },
            new TestEntry
            {
                TestId = "2",
                ExecutionId = "2",
                TestListId = null!
            }
        ],
        TestLists =
        [
            new TestList
            {
                Name = "List1",
                Id = null!
            },
            new TestList
            {
                Name = "List2",
                Id = null!
            }
        ],
        ResultSummary = new ResultSummary
        {
            Counters = new Counters
            {
                Total = 2,
                Passed = 1,
                Failed = 1
            },
            Output = new Output
            {
                StdOut = "test"
            },
            Outcome = "Failed",
            RunInfos = []
        },
        Times = null,
        TestSettings = null,
        Id = null!,
        Name = null!,
        RunUser = null!
    };
    
    
    private readonly TestRun _passedTestRun = new()
    {
        Results =
        [
            new UnitTestResult
            {
                Outcome = "Passed",
                Output = null!,
                ExecutionId = null!,
                TestId = null!,
                TestName = null!,
                ComputerName = null!,
                Duration = null!,
                TestType = null!,
                TestListId = null!,
                RelativeResultsDirectory = null!
            }
        ],
        TestDefinitions =
        [
            new UnitTest
            {
                TestMethod = new TestMethod
                {
                    ClassName = "Class1",
                    Name = "Method1",
                    CodeBase = null!,
                    AdapterTypeName = null!
                },
                Execution = null!,
                Name = null!,
                Storage = null!,
                Id = null!
            }
        ],
        TestEntries =
        [
            new TestEntry
            {
                TestId = "1",
                ExecutionId = "1",
                TestListId = null!
            }
        ],
        TestLists =
        [
            new TestList
            {
                Name = "List1",
                Id = null!
            }
        ],
        ResultSummary = new ResultSummary
        {
            Counters = new Counters
            {
                Total = 1,
                Passed = 1
            },
            Output = new Output
            {
                StdOut = "test"
            },
            Outcome = "Passed",
            RunInfos = []
        },
        Times = null,
        TestSettings = null,
        Id = null!,
        Name = null!,
        RunUser = null!
    };

    [Test]
    public void MergeTestRuns_When_Called_With_Null_TestRuns_Throws_ArgumentNullException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunMergerService>>();
        var service = new TestRunMergerService(mockLogger.Object);

        // Act
        void Act() => service.MergeTestRuns(null!);

        // Assert
        Assert.That(Act, Throws.ArgumentNullException);
    }

    [Test]
    public void MergeTestRuns_When_Called_With_Two_Failed_TestRuns_Returns_Merged_TestRun()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunMergerService>>();
        var service = new TestRunMergerService(mockLogger.Object);

        // Act
        var result = service.MergeTestRuns([_failedTestRun, _failedTestRun]);

        // Assert
        result.Results.Should().HaveCount(4);
        result.TestDefinitions.Should().HaveCount(4);
        result.TestEntries.Should().HaveCount(4);
        result.TestLists.Should().HaveCount(4);
        result.ResultSummary.Counters.Total.Should().Be(4);
        result.ResultSummary.Counters.Passed.Should().Be(2);
        result.ResultSummary.Counters.Failed.Should().Be(2);
        result.ResultSummary.Outcome.Should().Be("Failed");
        result.ResultSummary.Output.StdOut.Should().Be("test" + Environment.NewLine + "test");
    }
    
    [Test]
    public void MergeTestRuns_When_Called_With_Passed_And_Failed_TestRuns_Returns_Merged_TestRun()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunMergerService>>();
        var service = new TestRunMergerService(mockLogger.Object);

        // Act
        var result = service.MergeTestRuns([_passedTestRun, _failedTestRun]);

        // Assert
        result.Results.Should().HaveCount(3);
        result.TestDefinitions.Should().HaveCount(3);
        result.TestEntries.Should().HaveCount(3);
        result.TestLists.Should().HaveCount(3);
        result.ResultSummary.Counters.Total.Should().Be(3);
        result.ResultSummary.Counters.Passed.Should().Be(2);
        result.ResultSummary.Counters.Failed.Should().Be(1);
        result.ResultSummary.Outcome.Should().Be("Failed");
        result.ResultSummary.Output.StdOut.Should().Be("test" + Environment.NewLine + "test");
    }
    
    [Test]
    public void MergeTestRuns_When_Called_With_Two_Passed_TestRuns_Returns_Merged_TestRun()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunMergerService>>();
        var service = new TestRunMergerService(mockLogger.Object);

        // Act
        var result = service.MergeTestRuns([_passedTestRun, _passedTestRun]);

        // Assert
        result.Results.Should().HaveCount(2);
        result.TestDefinitions.Should().HaveCount(2);
        result.TestEntries.Should().HaveCount(2);
        result.TestLists.Should().HaveCount(2);
        result.ResultSummary.Counters.Total.Should().Be(2);
        result.ResultSummary.Counters.Passed.Should().Be(2);
        result.ResultSummary.Counters.Failed.Should().Be(0);
        result.ResultSummary.Outcome.Should().Be("Passed");
        result.ResultSummary.Output.StdOut.Should().Be("test" + Environment.NewLine + "test");
    }
}