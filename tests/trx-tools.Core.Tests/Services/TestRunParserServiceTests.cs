using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.Core.Exceptions;
using trx_tools.Core.Models;
using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.ResultSummary;
using trx_tools.Core.Models.TestDefinitions;
using trx_tools.Core.Models.TestEntries;
using trx_tools.Core.Services;
using Times = trx_tools.Core.Models.Times;

namespace trx_tools.Core.Tests.Services;

[TestFixture]
public class TestRunParserServiceTests
{
    [Test]
    public void ParseTestRun_When_TestRun_Is_Null_Should_Throws_ArgumentNullException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunParserService>>();
        var service = new TestRunParserService(mockLogger.Object);

        // Act
        void Act() => service.ParseTestRun(null!);

        // Assert
        Assert.That(Act, Throws.ArgumentNullException);
    }

    [Test]
    public void ParseTestRun_When_TestDefinition_NotFound_Throws_UnitTestDataNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunParserService>>();
        var service = new TestRunParserService(mockLogger.Object);
        var testRun = new TestRun
        {
            TestEntries =
            [
                new TestEntry
                {
                    TestId = "TestId",
                    ExecutionId = "ExecutionId",
                    TestListId = null!
                }
            ],
            TestDefinitions = [],
            Results = [],
            Times = null!,
            TestSettings = null!,
            TestLists = [],
            ResultSummary = null!,
            Id = null!,
            Name = null!,
            RunUser = null!
        };

        // Act
        void Act() => service.ParseTestRun(testRun);

        // Assert
        Assert.That(Act, Throws.TypeOf<UnitTestDataNotFoundException>());
    }

    [Test]
    public void ParseTestRun_When_TestResult_NotFound_Throws_TestResultDataNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunParserService>>();
        var service = new TestRunParserService(mockLogger.Object);
        var testRun = new TestRun
        {
            TestEntries =
            [
                new TestEntry
                {
                    TestId = "TestId",
                    ExecutionId = "ExecutionId",
                    TestListId = null!
                }
            ],
            TestDefinitions =
            [
                new UnitTest
                {
                    Id = "TestId",
                    Name = "TestName",
                    TestMethod = new TestMethod
                    {
                        ClassName = "ClassName",
                        Name = "MethodName",
                        CodeBase = null!,
                        AdapterTypeName = null!
                    },
                    Execution = null!,
                    Storage = null!
                }
            ],
            Results = [],
            Times = null!,
            TestSettings = null!,
            TestLists = [],
            ResultSummary = null!,
            Id = null!,
            Name = null!,
            RunUser = null!
        };

        // Act
        void Act() => service.ParseTestRun(testRun);

        // Assert
        Assert.That(Act, Throws.TypeOf<TestResultDataNotFoundException>());
    }

    [Test]
    public void ParseTestRun_When_TestId_Mismatch_Throws_TestIdsMismatchException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunParserService>>();
        var service = new TestRunParserService(mockLogger.Object);
        var testRun = new TestRun
        {
            TestEntries =
            [
                new TestEntry
                {
                    TestId = "TestId",
                    ExecutionId = "ExecutionId",
                    TestListId = null!
                }
            ],
            TestDefinitions =
            [
                new UnitTest
                {
                    Id = "TestId",
                    Name = "TestName",
                    TestMethod = new TestMethod
                    {
                        ClassName = "ClassName",
                        Name = "MethodName",
                        CodeBase = null!,
                        AdapterTypeName = null!
                    },
                    Execution = null!,
                    Storage = null!
                }
            ],
            Results =
            [
                new UnitTestResult
                {
                    TestId = "DifferentTestId",
                    ExecutionId = "ExecutionId",
                    Outcome = "Passed",
                    Duration = "00:00:00",
                    Output = null!,
                    TestName = null!,
                    ComputerName = null!,
                    TestType = null!,
                    TestListId = null!,
                    RelativeResultsDirectory = null!
                }
            ],
            Times = null!,
            TestSettings = null!,
            TestLists = [],
            ResultSummary = null!,
            Id = null!,
            Name = null!,
            RunUser = null!
        };

        // Act
        void Act() => service.ParseTestRun(testRun);

        // Assert
        Assert.That(Act, Throws.TypeOf<TestIdsMismatchException>());
    }

    [Test]
    public void ParseTestRun_When_TestResult_Is_Passed_Parses_TestResult()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunParserService>>();
        var service = new TestRunParserService(mockLogger.Object);
        var testRun = new TestRun
        {
            TestEntries =
            [
                new TestEntry
                {
                    TestId = "TestId",
                    ExecutionId = "ExecutionId",
                    TestListId = null!
                }
            ],
            TestDefinitions =
            [
                new UnitTest
                {
                    Id = "TestId",
                    Name = "TestName",
                    TestMethod = new TestMethod
                    {
                        ClassName = "ClassName_With_NameSpace",
                        Name = "MethodName",
                        CodeBase = "test",
                        AdapterTypeName = "test"
                    },
                    Execution = null!,
                    Storage = null!
                }
            ],
            Results =
            [
                new UnitTestResult
                {
                    TestId = "TestId",
                    ExecutionId = "ExecutionId",
                    Outcome = "Passed",
                    Duration = "00:00:01",
                    Output = null!,
                    TestName = null!,
                    ComputerName = null!,
                    TestType = null!,
                    TestListId = null!,
                    RelativeResultsDirectory = null!
                }
            ],
            Times = new Times
            {
                Creation = new DateTime(2000),
                Queuing = new DateTime(2000),
                Start = new DateTime(2000),
                Finish = new DateTime(2000)
            },
            TestSettings = null!,
            TestLists = [],
            ResultSummary = new ResultSummary
            {
                Counters = new Counters
                {
                    Aborted = 1,
                    Completed = 2,
                    Disconnected = 3,
                    Error = 4,
                    Failed = 5,
                    Inconclusive = 6,
                    InProgress = 7,
                    NotExecuted = 8,
                    NotRunnable = 9,
                    Passed = 10,
                    PassedButRunAborted = 11,
                    Pending = 12,
                    Timeout = 13,
                    Total = 14,
                    Warning = 15,
                    Executed = 16
                },
                Output = new Output { StdOut = "Output message" },
                Outcome = "Passed",
                RunInfos = []
            },
            Id = "1",
            Name = "testName",
            RunUser = "RunUser"
        };

        // Act
        var result = service.ParseTestRun(testRun);

        // Assert
        result.Name.Should().Be("testName");
        result.Id.Should().Be("1");
        result.RunUser.Should().Be("RunUser");
        result.Times.Should().NotBeNull();
        result.Times!.Creation.Should().Be(new DateTime(2000));
        result.Times.Queuing.Should().Be(new DateTime(2000));
        result.Times.Start.Should().Be(new DateTime(2000));
        result.Times.Finish.Should().Be(new DateTime(2000));
        result.ResultSummary.Should().NotBeNull();
        result.ResultSummary.Counters.Should().NotBeNull();
        result.ResultSummary.Counters.Aborted.Should().Be(1);
        result.ResultSummary.Counters.Completed.Should().Be(2);
        result.ResultSummary.Counters.Disconnected.Should().Be(3);
        result.ResultSummary.Counters.Error.Should().Be(4);
        result.ResultSummary.Counters.Failed.Should().Be(5);
        result.ResultSummary.Counters.Inconclusive.Should().Be(6);
        result.ResultSummary.Counters.InProgress.Should().Be(7);
        result.ResultSummary.Counters.NotExecuted.Should().Be(8);
        result.ResultSummary.Counters.NotRunnable.Should().Be(9);
        result.ResultSummary.Counters.Passed.Should().Be(10);
        result.ResultSummary.Counters.PassedButRunAborted.Should().Be(11);
        result.ResultSummary.Counters.Pending.Should().Be(12);
        result.ResultSummary.Counters.Timeout.Should().Be(13);
        result.ResultSummary.Counters.Total.Should().Be(14);
        result.ResultSummary.Counters.Warning.Should().Be(15);
        result.ResultSummary.Counters.Executed.Should().Be(16);
        result.ResultSummary.Output.Should().NotBeNull();
        result.ResultSummary.Output.StdOut.Should().Be("Output message");
        
        result.Results.Should().HaveCount(1);
        var firstResult = result.Results[0];
        firstResult.IsSuccess.Should().BeTrue();
        firstResult.Outcome.Should().Be("Passed");
        firstResult.Class.Should().Be("ClassName_With_NameSpace");
        firstResult.Name.Should().Be("MethodName");
        firstResult.Duration.Should().Be(TimeSpan.FromSeconds(1));
    }
}