using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.Models;

namespace EasyIdentity.Services.TokenProviders;

public class ClientCredentialsTokenProvider : IGrantTypeTokenProvider
{
    public string Name => GrantTypes.ClientCredentials;

    protected ITokenManager TokenManager { get; }

    protected IClientClaimsPrincipalFactory ClientClaimsPrincipalFactory { get; }

    public ClientCredentialsTokenProvider(ITokenManager tokenManager, IClientClaimsPrincipalFactory clientClaimsPrincipalFactory)
    {
        TokenManager = tokenManager;
        ClientClaimsPrincipalFactory = clientClaimsPrincipalFactory;
    }

    public virtual async Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = tokenRequestValidation.Subject;
        var scopes = tokenRequestValidation.Scopes;

        var principal = await ClientClaimsPrincipalFactory.CreateAsync(client, cancellationToken);

        return await TokenManager.GenerateAsync(client, claimsPrincipal: principal, scopes, requestData, cancellationToken: cancellationToken);
    }

    public virtual Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        TokenRequestValidationResult result;
        var clientId = requestData.ClientId.ToString();

        if (!string.IsNullOrWhiteSpace(clientId))
        {
            result = new TokenRequestValidationResult(clientId, requestData.Scopes);
        }
        else
        {
            result = TokenRequestValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
        }

        return Task.FromResult(result);
    }
}
