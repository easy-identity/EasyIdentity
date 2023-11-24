using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.Models;

namespace EasyIdentity.Services.TokenProviders;

public class ImplicitTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.Implicit;

    protected IAuthorizationCodeManager AuthorizationCodeManager { get; }
    protected IClientRedirectUrlValidator ClientRedirectUrlValidator { get; }
    protected ITokenManager TokenManager { get; }
    protected IUserProvider UserProvider { get; }

    public ImplicitTokenProvider(IAuthorizationCodeManager authorizationCodeManager, IClientRedirectUrlValidator clientRedirectUrlValidator, ITokenManager tokenManager, IUserProvider userProvider)
    {
        AuthorizationCodeManager = authorizationCodeManager;
        ClientRedirectUrlValidator = clientRedirectUrlValidator;
        TokenManager = tokenManager;
        UserProvider = userProvider;
    }

    public virtual async Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = tokenRequestValidation.Subject;
        var scopes = tokenRequestValidation.Scopes;

        var claims = await UserProvider.GetClaimsAsync(subject, scopes, cancellationToken);

        return await TokenManager.GenerateAsync(
            client,
            new ClaimsPrincipal(new ClaimsIdentity(claims)),
            scopes,
            requestData,
            true,
            true,
            cancellationToken);
    }

    public Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
