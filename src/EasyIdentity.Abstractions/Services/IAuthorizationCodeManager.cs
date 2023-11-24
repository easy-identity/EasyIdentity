using EasyIdentity.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeManager
{
    Task<string> GenerateAsync(Client client, ClaimsPrincipal claimsPrincipal, string[]? scopes, string redirectUri, string? codeChallengeMethod = null, string? codeChallenge = null, CancellationToken cancellationToken = default);

    Task<bool> ValidateCodeAsync(Client client, string code, string? verifier, string[]? scopes, string? redirectUri, CancellationToken cancellationToken = default);

    Task<string> FindSubjectAsync(Client client, string code, CancellationToken cancellationToken = default);

    Task<string[]> GetScopesAsync(Client client, string code, CancellationToken cancellationToken = default);
}
