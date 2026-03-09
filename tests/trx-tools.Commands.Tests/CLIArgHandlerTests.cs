using FluentAssertions;
using NUnit.Framework;

namespace trx_tools.Commands.Tests;

[TestFixture]
public class CLIArgHandlerTests
{
    [Test]
    public void Length_ShouldReturnCorrectCount()
    {
        // Arrange
        var args = new[] { "arg1", "arg2", "arg3" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.Length.Should().Be(3);
    }

    [Test]
    public void Length_WithEmptyArgs_ShouldReturnZero()
    {
        // Arrange
        var handler = new CLIArgHandler([]);

        // Act & Assert
        handler.Length.Should().Be(0);
    }

    #region GetFlag Tests

    [Test]
    public void GetFlag_WhenFlagExists_ShouldReturnTrue()
    {
        // Arrange
        var args = new[] { "dir", "output.html", "--only-latest" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetFlag("only-latest").Should().BeTrue();
    }

    [Test]
    public void GetFlag_WhenFlagDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var args = new[] { "dir", "output.html" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetFlag("only-latest").Should().BeFalse();
    }

    [Test]
    public void GetFlag_ShouldBeCaseInsensitive()
    {
        // Arrange
        var args = new[] { "--ONLY-LATEST" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetFlag("only-latest").Should().BeTrue();
        handler.GetFlag("ONLY-LATEST").Should().BeTrue();
        handler.GetFlag("Only-Latest").Should().BeTrue();
    }

    [Test]
    public void GetFlag_WithEmptyArgs_ShouldReturnFalse()
    {
        // Arrange
        var handler = new CLIArgHandler([]);

        // Act & Assert
        handler.GetFlag("any-flag").Should().BeFalse();
    }

    #endregion

    #region GetOpt Tests

    [Test]
    public void GetOpt_WhenOptionExists_ShouldReturnValue()
    {
        // Arrange
        var args = new[] { "--output", "file.html" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetOpt("output").Should().Be("file.html");
    }

    [Test]
    public void GetOpt_WhenOptionDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var args = new[] { "arg1", "arg2" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetOpt("output").Should().BeNull();
    }

    [Test]
    public void GetOpt_WhenOptionHasNoValue_ShouldReturnNull()
    {
        // Arrange
        var args = new[] { "--output" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetOpt("output").Should().BeNull();
    }

    [Test]
    public void GetOpt_WhenOptionFollowedByAnotherFlag_ShouldReturnNull()
    {
        // Arrange
        var args = new[] { "--output", "--another-flag" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetOpt("output").Should().BeNull();
    }

    [Test]
    public void GetOpt_ShouldBeCaseInsensitive()
    {
        // Arrange
        var args = new[] { "--OUTPUT", "value" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetOpt("output").Should().Be("value");
    }

    #endregion

    #region GetOptArr Tests

    [Test]
    public void GetOptArr_WhenOptionHasMultipleValues_ShouldReturnAllValues()
    {
        // Arrange
        var args = new[] { "--only-files", "file1.trx", "file2.trx", "file3.trx" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files");

        // Assert
        result.Should().BeEquivalentTo(["file1.trx", "file2.trx", "file3.trx"]);
    }

    [Test]
    public void GetOptArr_WithMaxLimit_ShouldReturnLimitedValues()
    {
        // Arrange
        var args = new[] { "--only-files", "file1.trx", "file2.trx", "file3.trx" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files", 2);

        // Assert
        result.Should().BeEquivalentTo(["file1.trx", "file2.trx"]);
    }

    [Test]
    public void GetOptArr_WhenValuesStopAtNextFlag_ShouldReturnValuesBeforeFlag()
    {
        // Arrange
        var args = new[] { "--only-files", "file1.trx", "file2.trx", "--another-flag", "value" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files");

        // Assert
        result.Should().BeEquivalentTo(["file1.trx", "file2.trx"]);
    }

    [Test]
    public void GetOptArr_WhenOptionDoesNotExist_ShouldReturnEmptyArray()
    {
        // Arrange
        var args = new[] { "arg1", "arg2" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files");

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetOptArr_WhenOptionHasNoValues_ShouldReturnEmptyArray()
    {
        // Arrange
        var args = new[] { "--only-files" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files");

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetOptArr_ShouldBeCaseInsensitive()
    {
        // Arrange
        var args = new[] { "--ONLY-FILES", "file1.trx" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetOptArr("only-files");

        // Assert
        result.Should().BeEquivalentTo(["file1.trx"]);
    }

    #endregion

    #region GetInitialUnnamedArgs Tests

    [Test]
    public void GetInitialUnnamedArgs_ShouldReturnArgsBeforeFirstFlag()
    {
        // Arrange
        var args = new[] { "dir", "output.html", "--only-latest" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetInitialUnnamedArgs();

        // Assert
        result.Should().BeEquivalentTo(["dir", "output.html"]);
    }

    [Test]
    public void GetInitialUnnamedArgs_WhenNoFlags_ShouldReturnAllArgs()
    {
        // Arrange
        var args = new[] { "dir", "output.html" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetInitialUnnamedArgs();

        // Assert
        result.Should().BeEquivalentTo(["dir", "output.html"]);
    }

    [Test]
    public void GetInitialUnnamedArgs_WhenFirstArgIsFlag_ShouldReturnEmptyArray()
    {
        // Arrange
        var args = new[] { "--only-latest", "file.trx" };
        var handler = new CLIArgHandler(args);

        // Act
        var result = handler.GetInitialUnnamedArgs();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetInitialUnnamedArgs_WithEmptyArgs_ShouldReturnEmptyArray()
    {
        // Arrange
        var handler = new CLIArgHandler([]);

        // Act
        var result = handler.GetInitialUnnamedArgs();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region Integration/Combined Tests

    [Test]
    public void ComplexArgs_ShouldParseCorrectly()
    {
        // Arrange - simulates: html path/to/dir output.html --only-latest --only-files file1.trx file2.trx
        var args = new[] { "path/to/dir", "output.html", "--only-latest", "--only-files", "file1.trx", "file2.trx" };
        var handler = new CLIArgHandler(args);

        // Act & Assert
        handler.GetInitialUnnamedArgs().Should().BeEquivalentTo(["path/to/dir", "output.html"]);
        handler.GetFlag("only-latest").Should().BeTrue();
        handler.GetOptArr("only-files").Should().BeEquivalentTo(["file1.trx", "file2.trx"]);
    }

    #endregion
}
