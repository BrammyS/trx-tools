using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.Core.Exceptions;
using trx_tools.Core.Models;
using trx_tools.Core.Services;
using trx_tools.Core.Tests.TestConverters;

namespace trx_tools.Core.Tests.Services;

[TestFixture]
public class TestRunTrxFileServiceTests
{
    [Test]
    public void ReadTestRun_Should_Return_ParsedTestRun()
    {
        // Arrange
        const int expectedResultsCount = 215;
        var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
        var service = new TestRunTrxFileService(mockLogger.Object);
        var trxFile = Path.Combine(AppContext.BaseDirectory, "TestFiles", "test_2025-02-11_17_41_50.trx");

        // Act
        var result = service.ReadTestRun(trxFile);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TestRun>();
        result.Name.Should().Be("test 2025-02-11 17:41:50");
        result.Results.Should().HaveCount(expectedResultsCount);
        result.TestEntries.Should().HaveCount(expectedResultsCount);
        result.TestDefinitions.Should().HaveCount(expectedResultsCount);
        result.TestLists.Should().HaveCount(2);
    }

    [Test]
    public void ReadTestRun_Returned_Object_Shoould_Match_Json()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
        var service = new TestRunTrxFileService(mockLogger.Object);
        var trxFile = Path.Combine(AppContext.BaseDirectory, "TestFiles", "test_2025-02-11_17_41_50.trx");

        var jsonFile = Path.Combine(AppContext.BaseDirectory, "TestFiles", "test_2025-02-11_17_41_50.json");
        var expectedJson = File.ReadAllText(jsonFile);

        // Act
        var result = service.ReadTestRun(trxFile);

        // Assert
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new UtcDateTimeConverter() }
        });
        json.ReplaceLineEndings().Trim().Should().Be(expectedJson.ReplaceLineEndings().Trim());
    }

    [Test]
    public void ReadTestRun_Should_Throw_Exception_When_File_Does_Not_Exist()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
        var service = new TestRunTrxFileService(mockLogger.Object);
        var trxFile = Path.Combine(AppContext.BaseDirectory, "TestFiles", "non_existent_file.trx");

        // Act
        Action act = () => service.ReadTestRun(trxFile);

        // Assert
        act.Should().Throw<TrxFileDoesNotExistException>();
    }

    [Test]
    public void ReadTestRun_Should_Throw_Exception_When_File_Is_Invalid()
    {
        var incorrectFile = Path.Combine(AppContext.BaseDirectory, "TestFiles", "invalid_file.trx");
        try
        {
            // Arrange
            File.WriteAllText(incorrectFile, "invalid content");
            var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
            var service = new TestRunTrxFileService(mockLogger.Object);

            // Act
            Action act = () => service.ReadTestRun(incorrectFile);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
        finally
        {
            File.Delete(incorrectFile);
        }
    }

    [Test]
    public void FindTrxFilesInDirectory_Should_Return_List_Of_Trx_Files()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
        var service = new TestRunTrxFileService(mockLogger.Object);
        var directory = Path.Combine(AppContext.BaseDirectory, "TestFiles");

        // Act
        var result = service.FindTrxFilesInDirectory(directory);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(5);
    }
    
    [Test]
    public void WriteHtmlReportAsync_Should_Write_Html_Report()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<TestRunTrxFileService>>();
        var service = new TestRunTrxFileService(mockLogger.Object);
        const string html = "<html><body><h1>Test</h1></body></html>";
        var path = Path.Combine(AppContext.BaseDirectory, "TestFiles", "test.html");

        // Act
        service.WriteHtmlReportAsync(path, html).Wait();

        // Assert
        File.Exists(path).Should().BeTrue();
        File.ReadAllText(path).Should().Be(html);
    }
}