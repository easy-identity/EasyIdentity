using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services.TokenProviders;

public class PasswordGrantTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.Password;

    protected ILogger<PasswordGrantTokenProvider> Logger { get; }
    protected IUserProvider UserIdentityProvider { get; }
    protected ITokenManager TokenManager { get; }

    public PasswordGrantTokenProvider(IUserProvider userIdentityProvider, ITokenManager tokenManager, ILogger<PasswordGrantTokenProvider> logger)
    {
        UserIdentityProvider = userIdentityProvider;
        TokenManager = tokenManager;
        Logger = logger;
    }

    /// <inheritdoc />
    public virtual async Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = tokenRequestValidation.Subject;
        var scopes = tokenRequestValidation.Scopes;

        var claims = await UserIdentityProvider.GetClaimsAsync(subject, scopes, cancellationToken);

        return await TokenManager.GenerateAsync(client, claimsPrincipal: new ClaimsPrincipal(new ClaimsIdentity(claims)), scopes, requestData, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var userName = requestData.Username.ToString();
        var password = requestData.Password.ToString();

        if (string.IsNullOrWhiteSpace(userName))
        {
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        var verificationResult = await UserIdentityProvider.ValidatePasswordAsync(userName, password, requestData, cancellationToken);

        if (!verificationResult.Succeeded)
        {
            Logger.LogWarning("User '{UserName}' password is invalid. {Fail}", userName, verificationResult.Fail);
            return TokenRequestValidationResult.Error(Error.Create(Error.INVALID_GRANT));
        }

        return new TokenRequestValidationResult(verificationResult.Subject, requestData.Scopes);
    }
}