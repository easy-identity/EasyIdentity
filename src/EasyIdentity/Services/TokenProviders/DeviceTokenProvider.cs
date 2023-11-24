using EasyIdentity.Constants;
using EasyIdentity.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services.TokenProviders;

public class DeviceTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.DeviceCode;

    protected IDeviceFlowManager DeviceCodeManager { get; }
    protected ITokenManager TokenManager { get; }
    protected IUserProvider UserIdentityProvider { get; }

    public DeviceTokenProvider(IDeviceFlowManager deviceCodeManager, ITokenManager tokenManager, IUserProvider userIdentityProvider)
    {
        DeviceCodeManager = deviceCodeManager;
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
        var deviceCode = requestData.DeviceCode;

        if (!string.IsNullOrWhiteSpace(deviceCode))
        {
            // TODO validate request speed.

            var validationResult = await DeviceCodeManager.ValidateDeviceCodeAsync(client, deviceCode!, requestData, cancellationToken);

            var scopes = await DeviceCodeManager.GetScopesByDeviceCodeAsync(client, deviceCode);

            if (validationResult.Succeeded)
            {
                switch (validationResult.State)
                {
                    case DeviceCodeAuthorizedState.Granted:
                        return new TokenRequestValidationResult(validationResult.Subject, scopes);
                    case DeviceCodeAuthorizedState.Pending:
                        return TokenRequestValidationResult.Error(Error.Create("authorization_pending"));
                    case DeviceCodeAuthorizedState.Declined:
                        return TokenRequestValidationResult.Error(Error.Create("authorization_declined"));
                    case DeviceCodeAuthorizedState.Expired:
                        return TokenRequestValidationResult.Error(Error.Create("expired_token"));
                }
            }
        }

        return TokenRequestValidationResult.Error(Error.Create("bad_verification_code"));
    }
}
