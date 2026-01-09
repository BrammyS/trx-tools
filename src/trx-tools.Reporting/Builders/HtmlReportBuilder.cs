using System.Text;
using trx_tools.Core;
using trx_tools.Core.Models.Parsed;
using trx_tools.HtmlReporting.Extensions;

namespace trx_tools.HtmlReporting.Builders;

public class HtmlReportBuilder
{
    private uint _totalTests;
    private uint _passedTests;
    private uint _failedTests;
    private uint _skippedTests;
    private bool _includeOutput;
    private bool _pretty;
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

    public HtmlReportBuilder WithIncludedOutput(bool includeOutput)
    {
        _includeOutput = includeOutput;
        return this;
    }

    public HtmlReportBuilder WithPretty(bool pretty)
    {
        _pretty = pretty;
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
        if (_pretty)
        {
            return BuildPretty();
        }

        var sb = new StringBuilder();
        sb.AppendLine("<html><head><style>");
        sb.AppendLine(".list-row,.row{width:100%;cursor:pointer}body{font-family:Calibri,Verdana,Arial,sans-serif;background-color:#fff;color:#000}h2{margin-top:15px;margin-bottom:10px}pre{white-space:pre-wrap}.testResult{word-wrap:break-word}.summary{font-family:monospace;display:-webkit-flex;-webkit-flex-wrap:wrap;display:flex;flex-wrap:wrap}.row{border:2px solid #fff;background-color:#d7e9fa}.inner-row,.list-row{background-color:#fff;border:2px solid #fff}.inner-row{padding-left:1%;margin-left:1%}.block{width:150px}.leaf-division{border:2px solid #fff;background-color:#e6eff7}.skip{color:#ffa500}.pass{color:#0c0}.fail{color:#c00}.error-message,.error-stack-trace{color:brown}.duration{float:right;padding-right:1%}.pass-percentage,.test-run-time,.total-tests{font-size:30px}.error-info{margin-left:16px}");
        sb.AppendLine("</style></head><body>");
        sb.AppendLine("<h1>Test run details</h1>");
        sb.AppendLine("<div class='summary'>");
        sb.AppendLine($"<div class='block'><span>Total tests</span><div class='total-tests'>{_totalTests}</div></div>");
        sb.AppendLine($"<div class='block'><span>Passed&nbsp;&nbsp;:&nbsp;</span><span class='passedTests'>{_passedTests}</span>");
        sb.AppendLine($"<br><span>Failed&nbsp;&nbsp;:&nbsp;</span><span class='failedTests'>{_failedTests}</span>");
        sb.AppendLine($"<br><span>Skipped :&nbsp;</span><span class='skippedTests'>{_skippedTests}</span></div>");
        sb.AppendLine($"<div class='block'><span>Pass percentage</span><div class='pass-percentage'>{CalculatePassPercentage():F0} %</div><br></div>");
        sb.AppendLine($"<div class='block'><span>Run duration</span><div class='test-run-time'>{_runDuration.ToHumanReadableTimeSpan()}</div></div>");
        sb.AppendLine("</div>");

        if (_failedTests > 0)
        {
            sb.AppendLine("<h2>Failed Results</h2>");
            AddTestResults(sb, _testResults.Where(x => x.IsFailed).ToList(), true);
        }

        sb.AppendLine("<h2>All Results</h2>");
        AddTestResults(sb, _testResults, false);

        sb.AppendLine("<div><h2>Informational messages</h2>");
        foreach (var msg in _messages)
        {
            sb.AppendLine($"<span>{msg.ReplaceLineEndings("<br>")}</span><br>");
        }

        sb.AppendLine("</div></body></html>");

        return sb.ToString();
    }

    private string BuildPretty()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Test Execution Report</title><style>");
        sb.AppendLine(":root{--bg-color:#f7f9fc;--card-bg:#ffffff;--text-primary:#1f2937;--text-secondary:#6b7280;--border-color:#e5e7eb;--accent-color:#3b82f6;--success-bg:#d1fae5;--success-text:#065f46;--error-bg:#fee2e2;--error-text:#991b1b;--skip-bg:#f3f4f6;--skip-text:#374151}body{font-family:'Segoe UI',system-ui,-apple-system,sans-serif;background:var(--bg-color);color:var(--text-primary);margin:0;padding:20px;line-height:1.5}.container{max-width:1200px;margin:0 auto}.header{display:flex;justify-content:space-between;align-items:center;margin-bottom:24px}.stats-grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(200px,1fr));gap:16px;margin-bottom:32px}.stat-card{background:var(--card-bg);padding:20px;border-radius:8px;box-shadow:0 1px 3px rgba(0,0,0,0.1);text-align:center}.stat-value{font-size:2rem;font-weight:700;margin:8px 0}.stat-label{color:var(--text-secondary);font-size:0.875rem;text-transform:uppercase;letter-spacing:0.05em;font-weight:600}.stat-card.passed .stat-value{color:#10b981}.stat-card.failed .stat-value{color:#ef4444}.filter-bar{background:var(--card-bg);padding:16px;border-radius:8px;margin-bottom:24px;display:flex;gap:16px;align-items:center;flex-wrap:wrap;box-shadow:0 1px 3px rgba(0,0,0,0.1)}.search-input,.category-select{padding:10px 12px;border:1px solid var(--border-color);border-radius:6px;font-size:14px}.search-input{flex:1;min-width:200px}.category-select{min-width:180px;background-color:#fff}.test-group{margin-bottom:16px;background:var(--card-bg);border-radius:8px;overflow:hidden;box-shadow:0 1px 2px rgba(0,0,0,0.05)}.test-group summary{padding:14px 20px;cursor:pointer;background:#f9fafb;font-weight:600;list-style:none;display:flex;align-items:center;justify-content:space-between;user-select:none}.test-group summary::-webkit-details-marker{display:none}.test-group summary:hover{background:#f3f4f6}.test-list{border-top:1px solid var(--border-color)}.test-item{padding:16px 20px;border-bottom:1px solid var(--border-color);transition:background 0.2s}.test-item:last-child{border-bottom:none}.test-header{display:flex;justify-content:space-between;align-items:center;width:100%;cursor:pointer}.test-name{font-weight:500;display:flex;align-items:center;gap:12px}.test-duration{color:var(--text-secondary);font-size:0.875rem;white-space:nowrap;font-family:monospace}.badge{padding:4px 10px;border-radius:999px;font-size:0.75rem;font-weight:700;text-transform:uppercase}.badge.pass{background:var(--success-bg);color:var(--success-text)}.badge.fail{background:var(--error-bg);color:var(--error-text)}.badge.skip{background:var(--skip-bg);color:var(--skip-text)}.test-content{margin-top:12px;padding:16px;background:#1e1e1e;color:#e5e5e5;border-radius:6px;font-family:Consolas,Monaco,monospace;font-size:0.875rem;overflow-x:auto}.error-message{color:#f87171;font-weight:bold;margin-bottom:8px}.stack-trace{color:#9ca3af}pre{margin:0;white-space:pre-wrap}.hidden{display:none!important}");
        sb.AppendLine("</style></head><body><div class='container'>");
        
        // Header & Stats
        sb.AppendLine("<div class='header'><h1>Test Execution Report</h1><div style='color:var(--text-secondary)'>Generated at " + DateTime.Now.ToString("g") + "</div></div>");
        sb.AppendLine("<div class='stats-grid'>");
        sb.AppendLine($"<div class='stat-card'> <div class='stat-label'>Total Tests</div> <div class='stat-value'>{_totalTests}</div> </div>");
        sb.AppendLine($"<div class='stat-card passed'> <div class='stat-label'>Passed</div> <div class='stat-value'>{_passedTests}</div> </div>");
        sb.AppendLine($"<div class='stat-card failed'> <div class='stat-label'>Failed</div> <div class='stat-value'>{_failedTests}</div> </div>");
        sb.AppendLine($"<div class='stat-card'> <div class='stat-label'>Pass Percentage</div> <div class='stat-value'>{CalculatePassPercentage():F0}%</div> </div>");
        sb.AppendLine($"<div class='stat-card'> <div class='stat-label'>Duration</div> <div class='stat-value' style='font-size:1.5rem'>{_runDuration.ToHumanReadableTimeSpan()}</div> </div>");
        sb.AppendLine("</div>");

        // Filter Bar
        var categories = _testResults.Select(r => r.Category).Distinct().Where(c => !string.IsNullOrEmpty(c)).OrderBy(c => c).ToList();
        sb.AppendLine("<div class='filter-bar'>");
        sb.AppendLine("<input type='text' id='searchInput' class='search-input' placeholder='Search tests...'>");
        sb.AppendLine("<select id='categorySelect' class='category-select'><option value=''>All Categories</option>");
            foreach(var cat in categories) sb.AppendLine($"<option value='{System.Net.WebUtility.HtmlEncode(cat)}'>{System.Net.WebUtility.HtmlEncode(cat)}</option>");
        sb.AppendLine("</select>");
        sb.AppendLine("<select id='statusSelect' class='category-select'><option value=''>All Statuses</option><option value='pass'>Passed</option><option value='fail'>Failed</option><option value='skip'>Skipped</option></select>");
        
        // Checkbox for StdOut
        if (_includeOutput)
        {
            sb.AppendLine("<label style='display:flex;align-items:center;gap:6px;font-size:14px;background:#fff;padding:8px 12px;border:1px solid var(--border-color);border-radius:6px;cursor:pointer;user-select:none'>");
            sb.AppendLine("<input type='checkbox' id='showOutputCheck'> Show stdout");
            sb.AppendLine("</label>");
        }
        
        sb.AppendLine("</div>");

        // Info Messages
        if(_messages.Any()){
             sb.AppendLine("<div style='margin-bottom:24px;padding:16px;background:#fff;border-radius:8px;border:1px solid var(--border-color)'><strong>Messages:</strong><br>");
             foreach(var msg in _messages) sb.AppendLine($"<div>{msg}</div>");
             sb.AppendLine("</div>");
        }

        // Test Results
        var groupedResults = _testResults.GroupBy(r => r.Codebase).OrderBy(g => g.Key);
        
        sb.AppendLine("<div id='testResults'>");
        foreach(var group in groupedResults)
        {
            sb.AppendLine("<details class='test-group' open>");
            sb.AppendLine($"<summary>{System.IO.Path.GetFileName(group.Key)} <span style='font-size:0.8em;color:var(--text-secondary)'>{group.Count()} tests</span></summary>");
            sb.AppendLine("<div class='test-list'>");
            
            foreach(var result in group)
            {
                var statusClass = GetTestResultCssClass(result);
                var statusIcon = result.IsSuccess ? "PASS" : (result.Outcome == Constants.NotExecutedTestRunOutcome ? "SKIP" : "FAIL");
                var catAttr = System.Net.WebUtility.HtmlEncode(result.Category);
                
                sb.AppendLine($"<div class='test-item' data-category='{catAttr}' data-status='{statusClass}' data-name='{System.Net.WebUtility.HtmlEncode(result.Name)}'>");
                
                // Header (Click to toggle details)
                sb.AppendLine("<details>");
                sb.AppendLine("<summary class='test-header'>");
                sb.AppendLine($"<div class='test-name'><span class='badge {statusClass}'>{statusIcon}</span> {System.Net.WebUtility.HtmlEncode(result.Class)}.{System.Net.WebUtility.HtmlEncode(result.Name)}</div>");
                sb.AppendLine($"<div class='test-duration'>{result.Duration.ToHumanReadableTimeSpan()}</div>");
                sb.AppendLine("</summary>");
                
                // Details content
                if(result.IsFailed || (_includeOutput && !string.IsNullOrWhiteSpace(result.Output?.StdOut)))
                {
                    sb.AppendLine("<div class='test-content'>");
                    if(result.IsFailed)
                    {
                        sb.AppendLine($"<div class='error-message'>{System.Net.WebUtility.HtmlEncode(result.Output.ErrorInfo.Message)}</div>");
                         if(!string.IsNullOrEmpty(result.Output.ErrorInfo.StackTrace))
                            sb.AppendLine($"<div class='stack-trace'>{System.Net.WebUtility.HtmlEncode(result.Output.ErrorInfo.StackTrace)}</div>");
                    }
                    if(_includeOutput && !string.IsNullOrWhiteSpace(result.Output?.StdOut))
                    {
                        if(result.IsFailed) sb.AppendLine("<hr style='border:0;border-top:1px solid #444;margin:12px 0'>");
                        sb.AppendLine($"<div class='std-out-content' style='display:none'><strong>Standard Output:</strong><br><pre>{System.Net.WebUtility.HtmlEncode(result.Output.StdOut)}</pre></div>");
                    }
                    sb.AppendLine("</div>");
                }
                
                sb.AppendLine("</details>");
                sb.AppendLine("</div>"); // End test-item
            }
            sb.AppendLine("</div>"); // End test-list
            sb.AppendLine("</details>"); // End test-group
        }
        sb.AppendLine("</div>");

        // Scripts
        sb.AppendLine(@"<script>
            const searchInput = document.getElementById('searchInput');
            const categorySelect = document.getElementById('categorySelect');
            const statusSelect = document.getElementById('statusSelect');
            const groups = document.querySelectorAll('.test-group');
            const showOutputCheck = document.getElementById('showOutputCheck');

            function filterTests() {
                const term = searchInput.value.toLowerCase();
                const category = categorySelect.value;
                const status = statusSelect.value;

                groups.forEach(group => {
                    let visibleCount = 0;
                    const items = group.querySelectorAll('.test-item');
                    
                    items.forEach(item => {
                        const name = item.dataset.name.toLowerCase();
                        const itemCat = item.dataset.category;
                        const itemStatus = item.dataset.status;
                        
                        const matchesSearch = name.includes(term);
                        const matchesCategory = !category || itemCat === category;
                        const matchesStatus = !status || itemStatus === status;
                        
                        if (matchesSearch && matchesCategory && matchesStatus) {
                            item.classList.remove('hidden');
                            visibleCount++;
                        } else {
                            item.classList.add('hidden');
                        }
                    });

                    if (visibleCount === 0) {
                        group.classList.add('hidden');
                    } else {
                        group.classList.remove('hidden');
                    }
                });
            }

            searchInput.addEventListener('input', filterTests);
            categorySelect.addEventListener('change', filterTests);
            statusSelect.addEventListener('change', filterTests);

            function toggleOutput() {
                 if(!showOutputCheck) return;
                 const isChecked = showOutputCheck.checked;
                 document.querySelectorAll('.std-out-content').forEach(el => {
                     el.style.display = isChecked ? 'block' : 'none';
                 });
            }
            if(showOutputCheck) showOutputCheck.addEventListener('change', toggleOutput);
        </script>");

        sb.AppendLine("</div></body></html>");
        return sb.ToString();
    }

    private void AddTestResults(StringBuilder sb, List<ParsedUnitTestResult> testRuns, bool open)
    {
        foreach (var codebaseGroup in testRuns.GroupBy(x => x.Codebase))
        {
            sb.AppendLine($"<details{GetOpenDetailTagWhenRequired(open)}>");
            sb.AppendLine($"<summary>{codebaseGroup.Key}</summary>");
            sb.AppendLine("<div class='inner-row'>");
            foreach (var testResult in codebaseGroup)
            {
                sb.AppendLine("<div class='leaf-division'>");
                sb.AppendLine($"<div class='testResult'><span class='{GetTestResultCssClass(testResult)}'>{GetTestResultIcon(testResult)}</span>");
                sb.AppendLine($"<span>{testResult.Class}_{testResult.Name}</span>");
                sb.AppendLine($"<div class='duration'><span>{testResult.Duration.ToHumanReadableTimeSpan()}</span></div>");

                if (testResult.IsFailed)
                {
                    sb.AppendLine("<div class='error-info'>");
                    sb.AppendLine($"Error: <span class='error-message'><pre>{testResult.Output.ErrorInfo.Message}</pre></span><br>");
                    sb.AppendLine($"Stack trace: <span class='error-message'><pre>{testResult.Output.ErrorInfo.StackTrace}</pre></span><br>");
                    sb.AppendLine("</div>");
                }

                if (_includeOutput && !string.IsNullOrWhiteSpace(testResult.Output?.StdOut))
                {
                    sb.AppendLine("<div class='error-info'>");
                    sb.AppendLine($"<details><summary>Standard Output</summary><pre>{testResult.Output.StdOut}</pre></details>");
                    sb.AppendLine("</div>");
                }

                sb.AppendLine("</div></div>");
            }

            sb.AppendLine("</div>");
            sb.AppendLine("</details>");
        }
    }

    private static string GetOpenDetailTagWhenRequired(bool open)
    {
        return open ? " open=''" : "";
    }

    private static string GetTestResultIcon(ParsedUnitTestResult testResult)
    {
        if (testResult.IsSuccess) return "✔";
        return testResult.Outcome == Constants.NotExecutedTestRunOutcome ? "❢" : "✘";
    }

    private static string GetTestResultCssClass(ParsedUnitTestResult testResult)
    {
        if (testResult.IsSuccess) return "pass";
        return testResult.Outcome == Constants.NotExecutedTestRunOutcome ? "skip" : "fail";
    }

    private double CalculatePassPercentage()
    {
        if (_totalTests == 0) return 0;
        return _passedTests / (double)_totalTests * 100;
    }
}