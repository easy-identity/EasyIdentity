using System;
using System.Runtime.Serialization;

namespace EasyIdentity.Models;

public class ErrorException : Exception
{
    public Error? ErrorData { get; }

    public ErrorException(Error errorData)
    {
        ErrorData = errorData;
    }

    public ErrorException(string message) : base(message)
    {
        ErrorData = new Error(message);
    }

    public ErrorException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorData = new Error(message);
    }

    [Obsolete]
    protected ErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ErrorException()
    {
    }
}
