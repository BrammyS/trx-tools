using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using trx_tools.Core.Exceptions;
using trx_tools.Core.Models;
using trx_tools.Core.Services.Interfaces;

namespace trx_tools.Core.Services;

public class TestRunTrxFileService(ILogger<TestRunTrxFileService> logger) : ITestRunTrxFileService
{
    public TestRun ReadTestRun(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new TrxFileDoesNotExistException($"TRX file does not exist at path {filePath}");
        }

        var serializer = new XmlSerializer(typeof(TestRun), "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
        using var fs = File.OpenRead(filePath);
        return (TestRun)serializer.Deserialize(fs)!;
    }

    public string[] FindTrxFilesInDirectory(string directoryPath)
    {
        logger.LogInformation("Finding TRX files in directory {DirectoryPath}", directoryPath);
        return Directory.GetFiles(directoryPath, "*.trx", SearchOption.AllDirectories);
    }

    public Task WriteHtmlReportAsync(string path, string html)
    {
        logger.LogInformation("Writing HTML report to {Path}", path);
        return File.WriteAllTextAsync(path, html);
    }
}