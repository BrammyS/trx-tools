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
        sb.AppendLine(".list-row,.row{width:100%;cursor:pointer}body{font-family:Calibri,Verdana,Arial,sans-serif;background-color:#fff;color:#000}h2{margin-top:15px;margin-bottom:10px}pre{white-space:pre-wrap}.summary{font-family:monospace;display:-webkit-flex;-webkit-flex-wrap:wrap;display:flex;flex-wrap:wrap}.row{border:2px solid #fff;background-color:#d7e9fa}.inner-row,.list-row{background-color:#fff;border:2px solid #fff}.inner-row{padding-left:1%;margin-left:1%}.block{width:150px}.leaf-division{border:2px solid #fff;background-color:#e6eff7}.pass{color:#0c0}.fail{color:#c00}.error-message,.error-stack-trace{color:brown}.duration{float:right;padding-right:1%}.pass-percentage,.test-run-time,.total-tests{font-size:30px}.error-info{margin-left:16px}");
        sb.AppendLine("</style></head><body>");
        sb.AppendLine("<h1>Test run details</h1>");
        sb.AppendLine("<div class='summary'>");
        sb.AppendLine($"<div class='block'><span>Total tests</span><div class='total-tests'>{_totalTests}</div></div>");
        sb.AppendLine($"<div class='block'><span>Passed :</span><span class='passedTests'>{_passedTests}</span>");
        sb.AppendLine($"<br><span>Failed :</span><span class='failedTests'>{_failedTests}</span>");
        sb.AppendLine($"<br><span>Skipped :</span><span class='skippedTests'>{_skippedTests}</span></div>");
        sb.AppendLine($"<div class='block'><span>Pass percentage</span><div class='pass-percentage'>{CalculatePassPercentage():F2} %</div><br></div>");
        sb.AppendLine($"<div class='block'><span>Run duration</span><div class='test-run-time'>{_runDuration.ToHumanReadableTimeSpan()}</div></div>");
        sb.AppendLine("</div>");

        if (_failedTests > 1)
        {
            sb.AppendLine("<h2>Failed Results</h2><details open=''>");
            AddTestResults(sb, _testResults.Where(x => !x.IsSuccess).ToList());
            sb.AppendLine("</details>");
        }

        sb.AppendLine("<h2>All Results</h2><details>");
        AddTestResults(sb, _testResults);
        sb.AppendLine("</details>");
        
        sb.AppendLine("<div><h2>Informational messages</h2>");
        foreach (var msg in _messages)
        {
            sb.AppendLine($"<span>{msg}</span><br>");
        }
        sb.AppendLine("</div></body></html>");
        
        return sb.ToString();
    }

    private static void AddTestResults(StringBuilder sb, List<ParsedUnitTestResult> testRuns)
    {
        sb.AppendLine("<summary>Test Results</summary>");
        sb.AppendLine("<div class='inner-row'>");

        foreach (var result in testRuns)
        {
            sb.AppendLine("<div class='leaf-division'>");
            sb.AppendLine($"<div><span class='{(result.IsSuccess ? "pass" : "fail")}'>{(result.IsSuccess ? "✔" : "✖")}</span>");
            sb.AppendLine($"<span>{result.FullName}</span>");
            sb.AppendLine($"<div class='duration'><span>{result.Duration.ToHumanReadableTimeSpan()}</span></div>");
            
            if (!result.IsSuccess)
            {
                sb.AppendLine("<div class='error-info'>");
                sb.AppendLine($"Error: <span class='error-message'><pre>{result.Output.ErrorInfo.Message}</pre></span><br>");
                sb.AppendLine($"Stack trace: <span class='error-message'><pre>{result.Output.ErrorInfo.StackTrace}</pre></span><br>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</div></div>");
        }
        
        sb.AppendLine("</div>");
    }
    
    private double CalculatePassPercentage()
    {
        if (_totalTests == 0) return 0;
        return _passedTests / (double)_totalTests * 100;
    }
}