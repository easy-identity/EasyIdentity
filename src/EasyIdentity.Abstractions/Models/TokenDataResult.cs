using System;

namespace EasyIdentity.Models;

public class TokenGenerated
{
    public string AccessToken { get; set; } = null!;
    public TimeSpan AccessTokenLifetime { get; set; }
    public string? IdentityToken { get; set; }
    public string? RefreshToken { get; set; }
    public string[]? Scopes { get; set; }
    public string? TokenType { get; set; }
}