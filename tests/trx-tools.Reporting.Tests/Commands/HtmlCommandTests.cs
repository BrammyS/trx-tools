﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using trx_tools.HtmlReporting.Commands;
using trx_tools.HtmlReporting.Services.Interfaces;

namespace trx_tools.HtmlReporting.Tests.Commands;

[TestFixture]
public class HtmlCommandTests
{
    [Test]
    public void Name_Should_Return_Html()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HtmlCommand>>();
        var mockIHtmlReportingService = new Mock<IHtmlReportingService>();
        var command = new HtmlCommand(mockLogger.Object, mockIHtmlReportingService.Object);

        // Act
        var result = command.Name;

        // Assert
        result.Should().Be("html");
    }
    
    [Test]
    public void Description_Should_Return_Generate_HTML_report_from_TRX_file()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HtmlCommand>>();
        var mockIHtmlReportingService = new Mock<IHtmlReportingService>();
        var command = new HtmlCommand(mockLogger.Object, mockIHtmlReportingService.Object);

        // Act
        var result = command.Description;

        // Assert
        result.Should().Be("Generate HTML report from TRX file(s). Example: trx-tools.Reporting html path/to/trx/directory output.html");
    }
    
    [Test]
    public async Task ExecuteAsync_Should_Call_GenerateHtmlReportAsync_When_Valid_Input()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HtmlCommand>>();
        var mockIHtmlReportingService = new Mock<IHtmlReportingService>();
        var command = new HtmlCommand(mockLogger.Object, mockIHtmlReportingService.Object);
        var args = new[] { "path/to/trx/directory", "output.html" };

        // Act
        await command.ExecuteAsync(args);

        // Assert
        mockIHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(args[0], args[1]), Times.Once);
    }
    
    [Test]
    public async Task ExecuteAsync_Should_Not_Call_GenerateHtmlReportAsync_When_Invalid_Number_Of_Arguments()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HtmlCommand>>();
        var mockIHtmlReportingService = new Mock<IHtmlReportingService>();
        var command = new HtmlCommand(mockLogger.Object, mockIHtmlReportingService.Object);
        var args = new[] { "path/to/trx/directory" };

        // Act
        await command.ExecuteAsync(args);

        // Assert
        mockIHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
    
    [Test]
    public async Task ExecuteAsync_Should_Not_Call_GenerateHtmlReportAsync_When_Output_File_Is_Not_HTML()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<HtmlCommand>>();
        var mockIHtmlReportingService = new Mock<IHtmlReportingService>();
        var command = new HtmlCommand(mockLogger.Object, mockIHtmlReportingService.Object);
        var args = new[] { "path/to/trx/directory", "output.txt" };

        // Act
        await command.ExecuteAsync(args);

        // Assert
        mockIHtmlReportingService.Verify(x => x.GenerateHtmlReportAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}