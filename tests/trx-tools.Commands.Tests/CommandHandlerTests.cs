using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace trx_tools.Commands.Tests;

[TestFixture]
public class CommandHandlerTests
{
    private readonly List<ICommand> _commands = [new TestCommand()];
    
    [Test]
    public async Task HandleCommandAsync_Should_Not_Execute()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<CommandHandler>>();
        var commandHandler = new CommandHandler(mockLogger.Object, _commands);

        // Act
        await commandHandler.HandleCommandAsync(["unknown"]);
    }
    
    [Test]
    public async Task HandleCommandAsync_Should_Execute()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<CommandHandler>>();
        var commandHandler = new CommandHandler(mockLogger.Object, _commands);

        // Act
        var result = () => commandHandler.HandleCommandAsync(["test"]);
        
        // Assert
        await result.Should().ThrowAsync<NotImplementedException>();
    }
}