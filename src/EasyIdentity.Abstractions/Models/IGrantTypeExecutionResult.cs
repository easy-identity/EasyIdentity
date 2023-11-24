using System;

namespace EasyIdentity.Models;

public interface IGrantTypeExecutionResult
{
    bool Succeeded { get; }

    Exception? Failure { get; }
}