using System;

namespace EasyIdentity.Models;

public class TokenValidationResult : ValidationResult
{
    public TokenDescriber Token { get; } = null!;

    protected TokenValidationResult()
    {
    }

    public TokenValidationResult(TokenDescriber token)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    public new static TokenValidationResult Error(Error error)
    {
        return new TokenValidationResult() { Failure = error };
    }
}