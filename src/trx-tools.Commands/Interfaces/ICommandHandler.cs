namespace trx_tools.Commands.Abstraction.Interfaces;

public interface ICommandHandler
{
    Task HandleCommandAsync(string[] args);
}