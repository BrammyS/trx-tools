using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.Commands;
using trx_tools.Core.Services.Interfaces;
using trx_tools.HtmlReporting.Commands;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Tests.Commands;

[TestFixture]
public class HtmlCommandTests
{
    private Mock<ILogger<HtmlCommand>> _mockLogger = null!;
    private Mock<IHtmlReportingService> _mockHtmlReportingService = null!;
    private Mock<ITestRunTrxFileService> _mockFileService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<HtmlCommand>>();
        _mockHtmlReportingService = new Mock<IHtmlReportingService>();
        _mockFileService = new Mock<ITestRunTrxFileService>();
    }

    private HtmlCommand CreateCommand() => new(_mockLogger.Object, _mockHtmlReportingService.Object, _mockFileService.Object);

    [Test]
    public void Name_Should_Return_Html()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = command.Name;

        // Assert
        result.Should().Be("html");
    }

    [Test]
    public void Description_Should_Return_Generate_HTML_report_from_TRX_file()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = command.Description;

        // Assert
        result.Should().Be("Generate HTML report from TRX file(s). Example: trx-tools.Reporting html path/to/trx/directory output.html [--only-latest] [--only-files file1.trx file2.trx]");
    }

    [Test]
    public async Task ExecuteAsync_Should_Call_GenerateHtmlReportAsync_When_Valid_Input()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html" };

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(args[0], args[1], It.Is<IHtmlReportingService.ReportOptions>(o => o.latestTrxOnly == false && (o.onlyFiles == null || !o.onlyFiles.Any()))), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_Should_Not_Call_GenerateHtmlReportAsync_When_Invalid_Number_Of_Arguments()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory" };

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IHtmlReportingService.ReportOptions>()), Times.Never);
    }

    [Test]
    public async Task ExecuteAsync_Should_Not_Call_GenerateHtmlReportAsync_When_Output_File_Is_Not_HTML()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.txt" };

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IHtmlReportingService.ReportOptions>()), Times.Never);
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyLatestFlag_ShouldPassLatestTrxOnlyOptionAsTrue()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html", "--only-latest" };

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            "path/to/trx/directory",
            "output.html",
            It.Is<IHtmlReportingService.ReportOptions>(o => o.latestTrxOnly == true)), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyLatestFlagCaseInsensitive_ShouldPassLatestTrxOnlyOptionAsTrue()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html", "--ONLY-LATEST" };

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            "path/to/trx/directory",
            "output.html",
            It.Is<IHtmlReportingService.ReportOptions>(o => o.latestTrxOnly == true)), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyFilesFlag_ShouldPassOnlyFilesOption()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html", "--only-files", "file1.trx", "file2.trx" };

        // Mock file existence checks
        _mockFileService.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            "path/to/trx/directory",
            "output.html",
            It.Is<IHtmlReportingService.ReportOptions>(o =>
                o.onlyFiles != null &&
                o.onlyFiles.Count() == 2)), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyFilesFlag_WhenFilesExistInTrxDirectory_ShouldResolvePaths()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "trxdir", "output.html", "--only-files", "test.trx" };

        // Mock: file exists in trx directory
        _mockFileService.Setup(x => x.FileExists(It.Is<string>(p => p.Contains("trxdir") && p.Contains("test.trx")))).Returns(true);
        _mockFileService.Setup(x => x.FileExists(It.Is<string>(p => !p.Contains("trxdir") && p.Contains("test.trx")))).Returns(false);

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.Is<IHtmlReportingService.ReportOptions>(o =>
                o.onlyFiles != null &&
                o.onlyFiles.Any(f => f.Contains("trxdir")))), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyFilesFlag_WhenFileDoesNotExist_ShouldThrowFileNotFoundException()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html", "--only-files", "nonexistent.trx" };

        // Mock file does not exist
        _mockFileService.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

        // Act & Assert
        var act = async () => await command.ExecuteAsync(new CLIArgHandler(args));
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("*nonexistent.trx*");
    }

    [Test]
    public async Task ExecuteAsync_WithOnlyFilesFlagAndAbsolutePath_ShouldUsePathAsIs()
    {
        // Arrange
        var command = CreateCommand();
        var absolutePath = Path.Combine(Path.GetTempPath(), "test.trx");
        var args = new[] { "trxdir", "output.html", "--only-files", absolutePath };

        // Mock: absolute path file exists
        _mockFileService.Setup(x => x.FileExists(absolutePath)).Returns(true);

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.Is<IHtmlReportingService.ReportOptions>(o =>
                o.onlyFiles != null &&
                o.onlyFiles.Contains(absolutePath))), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WithBothFlags_ShouldPassBothOptions()
    {
        // Arrange
        var command = CreateCommand();
        var args = new[] { "path/to/trx/directory", "output.html", "--only-latest", "--only-files", "file1.trx" };

        _mockFileService.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

        // Act
        await command.ExecuteAsync(new CLIArgHandler(args));

        // Assert
        _mockHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(
            "path/to/trx/directory",
            "output.html",
            It.Is<IHtmlReportingService.ReportOptions>(o =>
                o.latestTrxOnly == true &&
                o.onlyFiles != null &&
                o.onlyFiles.Any())), Times.Once);
    }
}
