using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using trx_tools.Core.Exceptions;
using trx_tools.Core.Models;
using trx_tools.Core.Services.Interfaces;

namespace trx_tools.Core.Services;

public class TestRunTrxFileService : ITestRunTrxFileService
{
    private readonly ILogger<TestRunTrxFileService> _logger;

    public TestRunTrxFileService(ILogger<TestRunTrxFileService> logger)
    {
        _logger = logger;
    }

    public TestRun ReadTestRun(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new TrxFileDoesNotExistException($"TRX file does not exist at path {filePath}");
        }

        var serializer = new XmlSerializer(typeof(TestRun), "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
        using var fs = File.OpenRead(filePath);
        var deserializedTestRun = serializer.Deserialize(fs);
        if (deserializedTestRun is null)
        {
            throw new NullReferenceException("Deserialized test run is null");
        }

        return (TestRun)deserializedTestRun;
    }

    public string[] FindTrxFilesInDirectory(string directoryPath)
    {
        _logger.LogInformation("Finding TRX files in directory {DirectoryPath}", directoryPath);
        return Directory.GetFiles(directoryPath, "*.trx", SearchOption.AllDirectories);
    }
}