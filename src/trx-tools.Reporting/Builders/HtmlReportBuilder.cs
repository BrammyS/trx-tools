using System.Text;
using trx_tools.Core.Models.Parsed;
using trx_tools.HtmlReporting.Extensions;

namespace trx_tools.HtmlReporting.Builders;

public class HtmlReportBuilder
{
    private uint _totalTests;
    private uint _passedTests;
    private uint _failedTests;
    private uint _skippedTests;
    private TimeSpan _runDuration = TimeSpan.Zero;
    private readonly List<ParsedUnitTestResult> _testResults = [];
    private readonly List<string> _messages = [];

    public HtmlReportBuilder WithTotalTests(uint total)
    {
        _totalTests = total;
        return this;
    }

    public HtmlReportBuilder WithPassedTests(uint passed)
    {
        _passedTests = passed;
        return this;
    }

    public HtmlReportBuilder WithFailedTests(uint failed)
    {
        _failedTests = failed;
        return this;
    }

    public HtmlReportBuilder WithSkippedTests(uint skipped)
    {
        _skippedTests = skipped;
        return this;
    }

    public HtmlReportBuilder WithTestResult(ParsedUnitTestResult testResult)
    {
        _testResults.Add(testResult);
        _runDuration += testResult.Duration;
        return this;
    }

    public HtmlReportBuilder WithMessage(string message)
    {
        _messages.Add(message);
        return this;
    }

    public string Build()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html><head><style>");
        sb.AppendLine("body { font-family: Calibri, Verdana, Arial, sans-serif; background-color: White; color: Black; }");
        sb.AppendLine("h2 { margin-top: 15px; margin-bottom: 10px; }");
        sb.AppendLine(".summary { font-family: monospace; display: flex; flex-wrap: wrap; }");
        sb.AppendLine(".block { width: 150px; }");
        sb.AppendLine(".pass { color: #0c0; }");
        sb.AppendLine(".fail { color: #c00; }");
        sb.AppendLine(".leaf-division { border: 2px solid #ffffff; background-color:#e6eff7; }");
        sb.AppendLine(".duration { float: right; padding-right: 1%; }");
        sb.AppendLine(".total-tests, .test-run-time, .pass-percentage { font-size: 30px; }");
        sb.AppendLine("</style></head><body>");
        sb.AppendLine("<h1>Test run details</h1>");
        sb.AppendLine("<div class='summary'>");
        sb.AppendLine($"<div class='block'><span>Total tests</span><div class='total-tests'>{_totalTests}</div></div>");
        sb.AppendLine($"<div class='block'><span>Passed :</span><span class='passedTests'>{_passedTests}</span>");
        sb.AppendLine($"<br><span>Failed :</span><span class='failedTests'>{_failedTests}</span>");
        sb.AppendLine($"<br><span>Skipped :</span><span class='skippedTests'>{_skippedTests}</span></div>");
        sb.AppendLine($"<div class='block'><span>Run duration</span><div class='test-run-time'>{_runDuration.ToHumanReadableTimeSpan()}</div></div>");
        sb.AppendLine("</div>");

        sb.AppendLine("<h2>All Results</h2><details>");
        sb.AppendLine("<summary>Test Results</summary>");
        sb.AppendLine("<div>");

        foreach (var result in _testResults)
        {
            sb.AppendLine("<div class='leaf-division'>");
            sb.AppendLine($"<div><span class='{(result.IsSuccess ? "pass" : "fail")}'>{(result.IsSuccess ? "✔" : "✖")}</span>");
            sb.AppendLine($"<span>{result.FullName}</span>");
            sb.AppendLine($"<div class='duration'><span>{result.Duration.ToHumanReadableTimeSpan()}</span></div>");
            sb.AppendLine("</div></div>");
        }
        
        sb.AppendLine("</div></details>");
        sb.AppendLine("<div><h2>Informational messages</h2>");
        foreach (var msg in _messages)
        {
            sb.AppendLine($"<span>{msg}</span><br>");
        }
        sb.AppendLine("</div></body></html>");
        
        return sb.ToString();
    }
}