using System;
using System.Collections.Generic;

namespace EasyIdentity.Exceptions;

public class BusinessException : Exception
{
    private readonly Dictionary<string, string> _error = [];

    public string? Descirpiton { get; set; }

    public BusinessException()
    {
    }

    public BusinessException(string message, string descirpiton) : base(message)
    {
        Descirpiton = descirpiton;
    }

    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public BusinessException AddData(string key, string value)
    {
        _error[key] = value;

        return this;
    }
}
