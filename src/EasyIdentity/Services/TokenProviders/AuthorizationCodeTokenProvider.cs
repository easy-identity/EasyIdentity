using EasyIdentity.Constants;
using EasyIdentity.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services.TokenProviders;

public class AuthorizationCodeTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.AuthorizationCode;

    protected IAuthorizationCodeManager AuthorizationCodeManager { get; }
    protected IClientRedirectUrlValidator ClientRedirectUrlValidator { get; }
    protected ITokenManager TokenManager { get; }
    protected IUserProvider UserIdentityProvider { get; }

    public AuthorizationCodeTokenProvider(IAuthorizationCodeManager authorizationCodeManager, IClientRedirectUrlValidator clientRedirectUrlValidator, ITokenManager tokenManager, IUserProvider userIdentityProvider)
    {
        AuthorizationCodeManager = authorizationCodeManager;
        ClientRedirectUrlValidator = clientRedirectUrlValidator;
        TokenManager = tokenManager;
        UserIdentityProvider = userIdentityProvider;
    }

    public virtual async Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = tokenRequestValidation.Subject;
        var scopes = tokenRequestValidation.Scopes;

        var claims = await UserIdentityProvider.GetClaimsAsync(subject, scopes, cancellationToken);

        // TODO

        return await TokenManager.GenerateAsync(
            client,
            new ClaimsPrincipal(new ClaimsIdentity(claims)),
            scopes,
            requestData,
            true,
            true,
            cancellationToken);
    }

    public virtual async Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var code = requestData.Code.ToString();
        var redirectUri = requestData.RedirectUri;

        if (string.IsNullOrWhiteSpace(code))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        if (!await ClientRedirectUrlValidator.ValidateAsync(client, redirectUri, cancellationToken))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        if (!await AuthorizationCodeManager.ValidateCodeAsync(client, code, requestData.CodeVerifier, requestData.Scopes, redirectUri, cancellationToken))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_GRANT));
        }

        var subject = await AuthorizationCodeManager.FindSubjectAsync(client, code, cancellationToken);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_GRANT));
        }

        var scopes = await AuthorizationCodeManager.GetScopesAsync(client, code, cancellationToken);

        return new TokenRequestValidationResult(subject, scopes);
    }
}
