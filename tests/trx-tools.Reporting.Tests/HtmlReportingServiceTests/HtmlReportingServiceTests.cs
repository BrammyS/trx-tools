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
        var validationHtml = await File.ReadAllTextAsync("TestFiles/TestReport1.html");
        var mockLogger = new Mock<ILogger<HtmlReportingService>>();
        var mockTestRunTrxFileService = new Mock<ITestRunTrxFileService>();
        mockTestRunTrxFileService.Setup(x => x.FindTrxFilesInDirectory(It.IsAny<string>())).Returns(["path/to/trx/file"]);
        mockTestRunTrxFileService.Setup(x => x.WriteHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>())).Callback(ValidateHtml());
        var mockTestRunParserService = new Mock<ITestRunParserService>();
        mockTestRunParserService.Setup(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>())).Returns(GetFakeParsedTestRun());
        var mockTestRunMergerService = new Mock<ITestRunMergerService>();
        var service = new HtmlReportingService(mockLogger.Object, mockTestRunTrxFileService.Object, mockTestRunMergerService.Object, mockTestRunParserService.Object);

        // Act
        await service.GenerateHtmlReportAsync(trxDirectory, htmlFile);

        // Assert
        mockTestRunTrxFileService.Verify(x => x.FindTrxFilesInDirectory(It.IsAny<string>()), Times.Once);
        mockTestRunParserService.Verify(x => x.ParseTestRun(It.IsAny<Core.Models.TestRun>()), Times.Once);
        mockTestRunTrxFileService.Verify(x => x.WriteHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        return;

        Action<string, string> ValidateHtml()
        {
            return (path, html) =>
            {
                // Assert
                path.Should().Be(Path.Combine(Directory.GetCurrentDirectory(), htmlFile));
                html.Should().Be(validationHtml);
            };
        }
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