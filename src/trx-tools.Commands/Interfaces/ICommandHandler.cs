namespace trx_tools.Commands.Interfaces;

public interface ICommandHandler
{
    Task HandleCommandAsync(string[] args);
}