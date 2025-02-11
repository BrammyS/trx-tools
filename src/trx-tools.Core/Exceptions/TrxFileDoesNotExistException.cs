namespace trx_tools.Core.Exceptions;

public class TrxFileDoesNotExistException : Exception
{
    public TrxFileDoesNotExistException(string message) : base(message)
    {
    }
}