namespace EasyIdentity.Models;

public class TokenRequestValidationResult : ValidationResult
{
    public string Subject { get; } = null!;
    public string[]? Scopes { get; }

    protected TokenRequestValidationResult()
    {
    }

    public TokenRequestValidationResult(string subject, string[]? scopes = null)
    {
        Subject = subject;
        Scopes = scopes;
    }

    public static new TokenRequestValidationResult Error(Error error)
    {
        return new TokenRequestValidationResult() { Failure = error };
    }
}