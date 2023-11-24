using System;

namespace EasyIdentity.Models;

public class AuthorizationCode
{
    public string Subject { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    [Obsolete]
    public string Code { get; set; } = null!;
    public string[]? Scopes { get; set; }
    public string RedirectUri { get; set; } = null!;
    public string? CodeChallenge { get; set; }
    public string? CodeChallengeMethod { get; set; }
    public TimeSpan Lifetime { get; set; }
    public DateTimeOffset CreationTime { get; set; }
}
