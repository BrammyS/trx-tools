namespace trx_tools.Core.Exceptions;

public class UnitTestDataNotFoundException : FileNotFoundException
{
    public UnitTestDataNotFoundException(string message) : base(message)
    {
    }
}