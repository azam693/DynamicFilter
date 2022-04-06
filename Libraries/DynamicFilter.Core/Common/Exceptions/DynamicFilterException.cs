namespace DynamicFilter.Core.Common.Exceptions;

public class DynamicFilterException : Exception
{
    public DynamicFilterException(string message, Exception exception = null)
        : base(message, exception)
    {
        
    }
}
