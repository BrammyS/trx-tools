using Microsoft.Extensions.Logging;
using trx_tools.Commands.Interfaces;

namespace trx_tools.Commands;

public class CommandHandler(ILogger<CommandHandler> logger, IEnumerable<ICommand> commands) : ICommandHandler
{
    public async Task HandleCommandAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "help")
        {
            PrintHelp();
            return;
        }

        var commandName = args[0];
        var commandArgs = args.Skip(1).ToArray();
        await ExecuteCommandAsync(commandName, commandArgs);
    }

    private async Task ExecuteCommandAsync(string commandName, string[] args)
    {
        var command = commands.FirstOrDefault(c => c.Name == commandName);
        if (command == null)
        {
            logger.LogError("Command {CommandName} not found", commandName);
            return;
        }

        await command.ExecuteAsync(args);
    }

    private void PrintHelp()
    {
        Console.WriteLine("Available commands:");
        foreach (var command in commands)
        {
            Console.WriteLine($"{command.Name} - {command.Description}");
        }
    }
}