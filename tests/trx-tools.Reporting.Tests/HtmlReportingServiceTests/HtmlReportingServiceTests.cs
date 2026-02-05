using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.Core;
using trx_tools.Core.Models.Parsed;
using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.ResultSummary;
using trx_tools.Core.Services.Interfaces;
using trx_tools.HtmlReporting.Services;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Tests.HtmlReportingServiceTests;

[TestFixture]
public class HtmlReportingServiceTests
{
    [Test]
    public async Task GenerateHtmlReportAsync_Should_Call_GenerateHtmlReportAsync_When_Valid_Input()
    {
        // Arrange
        const string htmlFile = "output.html";
        const string trxDirectory = "path/to/trx/directory";
        var mockLogger = new Mock<ILogger<HtmlReportingService>>();
        var mockTestRunTrxFileService = new Mock<ITestRunTrxFileService>();
        mockTestRunTrxFileService.Setup(x => x.FindTrxFilesInDirectory(It.IsAny<string>())).Returns(["path/to/trx/file"]);
        mockTestRunTrxFileService.Setup(x => x.WriteHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        var mockTestRunParserService = new Mock<ITestRunParserService>();
        mockTestRunParserService.Setup(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>())).Returns(GetFakeParsedTestRun());
        var mockTestRunMergerService = new Mock<ITestRunMergerService>();
        var service = new HtmlReportingService(mockLogger.Object, mockTestRunTrxFileService.Object, mockTestRunMergerService.Object, mockTestRunParserService.Object);

        // Act
        await service.GenerateHtmlReportAsync(trxDirectory, htmlFile);

        // Assert
        mockTestRunTrxFileService.Verify(x => x.FindTrxFilesInDirectory(It.IsAny<string>()), Times.Once);
        mockTestRunParserService.Verify(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>()), Times.Once);
        mockTestRunTrxFileService.Verify(x => x.WriteHtmlReportAsync(
            It.Is<string>(p => p.EndsWith(htmlFile)),
            It.Is<string>(h => h.Contains("Test run details") && h.Contains("unitTestName1"))), Times.Once);
    }

    [Test]
    public async Task GenerateHtmlReportAsync_WithLatestOnly_ShouldOnlyUseLatestFile()
    {
        // Arrange
        const string htmlFile = "output.html";
        const string olderFile = "older.trx";
        const string newerFile = "newer.trx";
        var mockLogger = new Mock<ILogger<HtmlReportingService>>();
        var mockTestRunTrxFileService = new Mock<ITestRunTrxFileService>();

        // Mock file discovery
        mockTestRunTrxFileService.Setup(x => x.FindTrxFilesInDirectory(It.IsAny<string>())).Returns([olderFile, newerFile]);

        // Mock file timestamps - newerFile is more recent
        mockTestRunTrxFileService.Setup(x => x.GetFileLastWriteTime(olderFile)).Returns(DateTime.Now.AddDays(-1));
        mockTestRunTrxFileService.Setup(x => x.GetFileLastWriteTime(newerFile)).Returns(DateTime.Now);

        // Mock reading the newer file (which should be selected)
        mockTestRunTrxFileService.Setup(x => x.ReadTestRun(newerFile)).Returns(new Core.Models.TestRun
        {
            RunUser = "test",
            Times = null,
            TestSettings = null,
            Results = [],
            TestDefinitions = [],
            TestEntries = [],
            TestLists = [],
            ResultSummary = new Core.Models.ResultSummary.ResultSummary
            {
                Counters = new Core.Models.ResultSummary.Counters(),
                Output = null,
                Outcome = "Completed",
                RunInfos = []
            },
            Id = "test-id",
            Name = "test-name"
        });
        mockTestRunTrxFileService.Setup(x => x.WriteHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var mockTestRunParserService = new Mock<ITestRunParserService>();
        mockTestRunParserService.Setup(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>())).Returns(GetFakeParsedTestRun());
        var mockTestRunMergerService = new Mock<ITestRunMergerService>();
        var service = new HtmlReportingService(mockLogger.Object, mockTestRunTrxFileService.Object, mockTestRunMergerService.Object, mockTestRunParserService.Object);

        // Act
        await service.GenerateHtmlReportAsync("dir", htmlFile, new IHtmlReportingService.ReportOptions(latestTrxOnly: true));

        // Assert - only the newer file should be read
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(newerFile), Times.Once);
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(olderFile), Times.Never);
    }

    [Test]
    public async Task GenerateHtmlReportAsync_WithOnlyFiles_ShouldOnlyUseSpecifiedFiles()
    {
        // Arrange
        const string htmlFile = "output.html";
        var mockLogger = new Mock<ILogger<HtmlReportingService>>();
        var mockTestRunTrxFileService = new Mock<ITestRunTrxFileService>();
        
        var file1 = "file1.trx";
        var file2 = "file2.trx";

        mockTestRunTrxFileService.Setup(x => x.ReadTestRun(It.Is<string>(s => s.Contains(file1)))).Returns(new Core.Models.TestRun 
        { 
            RunUser = "test",
            Times = null,
            TestSettings = null,
            Results = [],
            TestDefinitions = [],
            TestEntries = [],
            TestLists = [],
            ResultSummary = new Core.Models.ResultSummary.ResultSummary
            {
                Counters = new Core.Models.ResultSummary.Counters(),
                Output = null,
                Outcome = "Completed",
                RunInfos = []
            },
            Id = "test-id",
            Name = "test-name"
        });
        mockTestRunTrxFileService.Setup(x => x.WriteHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        
        var mockTestRunParserService = new Mock<ITestRunParserService>();
        mockTestRunParserService.Setup(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>())).Returns(GetFakeParsedTestRun());
        var mockTestRunMergerService = new Mock<ITestRunMergerService>();
        var service = new HtmlReportingService(mockLogger.Object, mockTestRunTrxFileService.Object, mockTestRunMergerService.Object, mockTestRunParserService.Object);

        // Act
        await service.GenerateHtmlReportAsync("dir", htmlFile, new IHtmlReportingService.ReportOptions(onlyFiles: [file1]));

        // Assert
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(It.Is<string>(s => s.Contains(file1))), Times.Once);
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(It.Is<string>(s => s.Contains(file2))), Times.Never);
        mockTestRunTrxFileService.Verify(x => x.FindTrxFilesInDirectory(It.IsAny<string>()), Times.Never);
    }

    private static ParsedTestRun GetFakeParsedTestRun()
    {
        return new ParsedTestRun(
            new Core.Models.Times
            {
                Creation = default,
                Queuing = default,
                Start = default,
                Finish = default
            },
            [
                new ParsedUnitTestResult(true, "pass", "unitTestClass", "unitTestName1", "file.dll", TimeSpan.FromSeconds(1), new UnitTestResultOutput
                {
                    StdOut = "message",
                    ErrorInfo = null!
                }),
                new ParsedUnitTestResult(false, "fail", "unitTestClass", "unitTestName2", "file.dll", TimeSpan.FromSeconds(1), new UnitTestResultOutput
                {
                    StdOut = "message",
                    ErrorInfo = new ErrorInfo
                    {
                        Message = "error",
                        StackTrace = "stack"
                    }
                }),
                new ParsedUnitTestResult(false, Constants.NotExecutedTestRunOutcome, "unitTestClass", "unitTestName3", "file.dll", TimeSpan.FromSeconds(1), new UnitTestResultOutput
                {
                    StdOut = "message",
                    ErrorInfo = null!
                })
            ],
            new ResultSummary
            {
                Counters = new Counters
                {
                    Passed = 2,
                    Failed = 1,
                    Total = 4
                },
                Output = new Output
                {
                    StdOut = "message"
                },
                Outcome = null!,
                RunInfos = []
            },
            "id",
            "name",
            "runUser"
        );
    }
}