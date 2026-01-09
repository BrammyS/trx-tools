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
            It.Is<string>(h => h.Contains("Test run details") && h.Contains("unitTestName1") && !h.Contains("<summary>Standard Output</summary>"))), Times.Once);
    }

    [Test]
    public async Task GenerateHtmlReportAsync_Should_Include_StdOut_In_Html_When_IncludeOutput_Is_True()
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
        await service.GenerateHtmlReportAsync(trxDirectory, htmlFile, includeOutput: true);

        // Assert
        mockTestRunTrxFileService.Verify(x => x.WriteHtmlReportAsync(
            It.Is<string>(p => p.EndsWith(htmlFile)),
            It.Is<string>(h => h.Contains("<summary>Standard Output</summary>"))), Times.Once);
    }

    [Test]
    public async Task GenerateHtmlReportAsync_WithLatestOnly_ShouldOnlyUseLatestFile()
    {
        // Arrange
        const string htmlFile = "output.html";
        var mockLogger = new Mock<ILogger<HtmlReportingService>>();
        var mockTestRunTrxFileService = new Mock<ITestRunTrxFileService>();
        
        var file1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".trx");
        var file2 = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".trx");
        File.WriteAllText(file1, "old");
        File.SetLastWriteTime(file1, DateTime.Now.AddDays(-1));
        File.WriteAllText(file2, "new");
        File.SetLastWriteTime(file2, DateTime.Now);

        try
        {
            mockTestRunTrxFileService.Setup(x => x.FindTrxFilesInDirectory(It.IsAny<string>())).Returns([file1, file2]);
            mockTestRunTrxFileService.Setup(x => x.ReadTestRun(file2)).Returns(new Core.Models.TestRun 
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
            await service.GenerateHtmlReportAsync("dir", htmlFile, latestOnly: true);

            // Assert
            mockTestRunTrxFileService.Verify(x => x.ReadTestRun(file2), Times.Once);
            mockTestRunTrxFileService.Verify(x => x.ReadTestRun(file1), Times.Never);
        }
        finally
        {
            if (File.Exists(file1)) File.Delete(file1);
            if (File.Exists(file2)) File.Delete(file2);
        }
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
        await service.GenerateHtmlReportAsync("dir", htmlFile, onlyFiles: [file1]);

        // Assert
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(It.Is<string>(s => s.Contains(file1))), Times.Once);
        mockTestRunTrxFileService.Verify(x => x.ReadTestRun(It.Is<string>(s => s.Contains(file2))), Times.Never);
        mockTestRunTrxFileService.Verify(x => x.FindTrxFilesInDirectory(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task GenerateHtmlReportAsync_Should_Generate_Pretty_Report_When_Pretty_Is_True()
    {
        // Arrange
        const string htmlFile = "output-pretty.html";
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
        await service.GenerateHtmlReportAsync(trxDirectory, htmlFile, pretty: true);

        // Assert
        mockTestRunTrxFileService.Verify(x => x.WriteHtmlReportAsync(
            It.Is<string>(p => p.EndsWith(htmlFile)),
            It.Is<string>(h => h.Contains("class='stats-grid'") && h.Contains("class='filter-bar'") && h.Contains("Test Execution Report"))), Times.Once);
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
                new ParsedUnitTestResult(true, "pass", "unitTestClass", "unitTestName1", "file.dll", "Category1", TimeSpan.FromSeconds(1), new UnitTestResultOutput
                {
                    StdOut = "message",
                    ErrorInfo = null!
                }),
                new ParsedUnitTestResult(false, "fail", "unitTestClass", "unitTestName2", "file.dll", "Category1", TimeSpan.FromSeconds(1), new UnitTestResultOutput
                {
                    StdOut = "message",
                    ErrorInfo = new ErrorInfo
                    {
                        Message = "error",
                        StackTrace = "stack"
                    }
                }),
                new ParsedUnitTestResult(false, Constants.NotExecutedTestRunOutcome, "unitTestClass", "unitTestName3", "file.dll", "Category2", TimeSpan.FromSeconds(1), new UnitTestResultOutput
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