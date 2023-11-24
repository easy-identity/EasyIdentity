using System;

namespace EasyIdentity.Models;

public abstract class GrantTypeExecutionResult : IGrantTypeExecutionResult
{
    public bool Succeeded => Failure == null;

    public Exception? Failure { get; protected set; }
}
