using Microsoft.Extensions.Logging;
using trx_tools.Core.Extensions;
using trx_tools.Core.Models;
using trx_tools.Core.Services.Interfaces;
using trx_tools.HtmlReporting.Builders;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Services;

public class HtmlReportingService(
    ILogger<HtmlReportingService> logger,
    ITestRunTrxFileService fileService,
    ITestRunMergerService mergerService,
    ITestRunParserService parserService
) : IHtmlReportingService
{
    public async Task GenerateHtmlReportAsync(string trxDirectory, string outputFile, IHtmlReportingService.ReportOptions? options = default)
    {
        options ??=new();
        string[] trxFiles;
        if (options.onlyFiles?.Any() == true)
        {
            trxFiles = options.onlyFiles.ToArray();
        }
        else
        {
            trxFiles = fileService.FindTrxFilesInDirectory(Path.Combine(Directory.GetCurrentDirectory(), trxDirectory));
        }

        if (trxFiles.Length == 0)
        {
            logger.LogError("No TRX files found");
            return;
        }

        if (options.latestTrxOnly)
        {
            trxFiles = [trxFiles.OrderByDescending(File.GetLastWriteTime).First()];
        }

        var testRun = GetTestRun(trxFiles);
        var parsedTestRun = parserService.ParseTestRun(testRun);
        var htmlReport = new HtmlReportBuilder()
            .WithPassedTests(parsedTestRun.ResultSummary.Counters.Passed)
            .WithFailedTests(parsedTestRun.ResultSummary.Counters.Failed)
            .WithSkippedTests(parsedTestRun.ResultSummary.Counters.GetSkippedTestCount())
            .WithTotalTests(parsedTestRun.ResultSummary.Counters.Total);
        
        if (parsedTestRun.ResultSummary.Output is not null)
        {
            htmlReport.WithMessage(parsedTestRun.ResultSummary.Output.StdOut);
        }

        foreach (var unitTestResult in parsedTestRun.Results)
        {
            htmlReport.WithTestResult(unitTestResult);
        }

        await fileService.WriteHtmlReportAsync(Path.Combine(Directory.GetCurrentDirectory(), outputFile), htmlReport.Build());
    }

    private TestRun GetTestRun(string[] trxFiles)
    {
        var testRuns = trxFiles.Select(fileService.ReadTestRun).ToList();
        return testRuns.Count > 1 ? mergerService.MergeTestRuns(testRuns) : testRuns.Single();
    }
}