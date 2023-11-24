using EasyIdentity.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface ITokenManager
{
    Task<TokenGenerated> GenerateAsync(
        Client client,
        ClaimsPrincipal claimsPrincipal,
        string[]? scopes,
        IRequestCollection requestData,
        bool identityToken = false,
        bool refreshToken = false,
        CancellationToken cancellationToken = default);

    Task<TokenDescriber> CreateTokenAsync(Client client, string tokenType, string[]? scopes, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);

    Task<TokenValidationResult> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<TokenValidationResult> ValidateAccessTokenAsync(Client client, string accessToken, CancellationToken cancellationToken = default);

    Task<TokenValidationResult> ValidateRefreshTokenAsync(Client client, string refreshToken, CancellationToken cancellationToken = default);
}
