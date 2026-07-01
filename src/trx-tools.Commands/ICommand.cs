namespace trx_tools.Commands;

public interface ICommand
{
    string Name { get; }
    string Description { get; }

    Task ExecuteAsync(CLIArgHandler args);
}
