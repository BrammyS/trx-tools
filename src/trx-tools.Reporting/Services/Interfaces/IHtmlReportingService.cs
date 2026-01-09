namespace trx_tools.HtmlReporting.Services.Interfaces;

public interface IHtmlReportingService
{
    Task GenerateHtmlReportAsync(string trxDirectory, string outputFile, bool latestOnly = false, IEnumerable<string>? onlyFiles = null, bool includeOutput = false);
}