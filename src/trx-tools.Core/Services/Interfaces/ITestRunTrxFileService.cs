using trx_tools.Core.Models;

namespace trx_tools.Core.Services.Interfaces;

public interface ITestRunTrxFileService
{
    TestRun ReadTestRun(string filePath);
    string[] FindTrxFilesInDirectory(string directoryPath);
    Task WriteHtmlReportAsync(string combine, string build);
}