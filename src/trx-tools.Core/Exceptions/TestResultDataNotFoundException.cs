namespace trx_tools.Core.Exceptions;

public class TestResultDataNotFoundException : FileNotFoundException
{
    public TestResultDataNotFoundException(string message) : base(message)
    {
    }
}