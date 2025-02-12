namespace trx_tools.Core.Exceptions;

public class DuplicatedTestExecutionFoundException : Exception
{
    public DuplicatedTestExecutionFoundException(string message) : base(message)
    {
    }
}