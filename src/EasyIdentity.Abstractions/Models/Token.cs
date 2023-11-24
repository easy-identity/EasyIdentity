using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EasyIdentity.Models;

/// <summary>
///  Token Descriptor
/// </summary>
public class TokenDescriber
{
    public TokenDescriber(string id, string subject, string clientId)
    {
        Id = id;
        Subject = subject;
        ClientId = clientId;
    }

    public string Id { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string? Type { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string[]? Scopes { get; set; }
    public string? AccessTokenHash { get; set; }
    public string? AuthorizationCodeHash { get; set; }
    public string? StateHash { get; set; }
    public List<Claim> Claims { get; set; } = [];
    public TimeSpan Lifetime { get; set; }
    public DateTimeOffset CreationTime { get; set; }
}
