using EasyIdentity.Constants;
using EasyIdentity.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services.TokenProviders;

public class RefreshTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.RefreshToken;

    protected ITokenManager TokenManager { get; }
    protected IUserProvider UserIdentityProvider { get; }

    public RefreshTokenProvider(ITokenManager tokenManager, IUserProvider userIdentityProvider)
    {
        TokenManager = tokenManager;
        UserIdentityProvider = userIdentityProvider;
    }

    public virtual async Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = tokenRequestValidation.Subject;
        var scopes = tokenRequestValidation.Scopes;

        var claims = await UserIdentityProvider.GetClaimsAsync(subject, scopes, cancellationToken);

        return await TokenManager.GenerateAsync(client, claimsPrincipal: new ClaimsPrincipal(new ClaimsIdentity(claims)), scopes, requestData, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var clientId = requestData.ClientId;

        var token = requestData.RefreshToken;
        var scopes = requestData.Scopes;

        if (string.IsNullOrWhiteSpace(token))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        var validationResult = await TokenManager.ValidateRefreshTokenAsync(client, token!, cancellationToken);

        if (!validationResult.Succeeded)
        {
            return TokenRequestValidationResult.Error(validationResult.Failure!);
        }

        if (validationResult.Token.Scopes.Except(scopes).Any())
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_SCOPE));
        }

        return new TokenRequestValidationResult(validationResult.Token.Subject, requestData.Scopes);
    }
}
