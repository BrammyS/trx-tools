namespace trx_tools.Commands.Tests;

public class TestCommand : ICommand
{
    public string Name => "test";
    public string Description => "Test command";
    public Task ExecuteAsync(CLIArgHandler args)
    {
        throw new NotImplementedException();
    }
}