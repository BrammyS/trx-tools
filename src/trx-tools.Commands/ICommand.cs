namespace trx_tools.Commands.Abstraction;

public interface ICommand
{
    string Name { get; }
    string Description { get; }

    Task ExecuteAsync(string[] args);
}