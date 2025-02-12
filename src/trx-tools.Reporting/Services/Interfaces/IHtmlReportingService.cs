namespace trx_tools.HtmlReporting.Services.Interfaces;

public interface IHtmlReportingService
{
    Task GenerateHtmlReportAsync(string trxDirectory, string outputFile);
}