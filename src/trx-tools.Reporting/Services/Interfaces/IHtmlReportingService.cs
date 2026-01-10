namespace trx_tools.HtmlReporting.Services.Interfaces;


public interface IHtmlReportingService
{
    public record ReportOptions(bool latestTrxOnly=false, IEnumerable<string>? onlyFiles=null);
    Task GenerateHtmlReportAsync(string trxDirectory, string outputFile, ReportOptions? options = default);
}
