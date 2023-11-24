using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/connect/token", Method = "POST")]
public class TokenEndpointHandler : EndpointHandler
{
    public override string Name => "token";

    protected IClientRequestValidator ClientValidator { get; }
    protected IGrantTypeTokenProviderFactory GrantTypeProviderFactory { get; }
    protected IOptions<EasyIdentityOptions> Options { get; }

    public TokenEndpointHandler(
        IClientRequestValidator clientVerificationProvider,
        IGrantTypeTokenProviderFactory grantTypeProviderFactory,
        IOptions<EasyIdentityOptions> options)
    {
        ClientValidator = clientVerificationProvider;
        GrantTypeProviderFactory = grantTypeProviderFactory;
        Options = options;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var clientId = requestData.ClientId;
        var grantType = requestData.GrantType;

        // validate client 
        var clientVerificationResult = await ClientValidator.ValidateAsync(Name, clientId!, requestData, cancellationToken);

        if (!clientVerificationResult.Succeeded)
        {
            return new ErrorResult(requestData, clientVerificationResult.Failure!);
        }

        Logger.LogDebug("Client '{ClientId}' request token for grant type '{GantType}'", clientId, grantType);

        var grantTypeProvider = GrantTypeProviderFactory.Get(grantType!);

        // validate request
        var validationResult = await grantTypeProvider.ValidateAsync(clientVerificationResult.Client, requestData, cancellationToken);

        if (!validationResult.Succeeded)
        {
            Logger.LogWarning("Client '{ClientId}' request token validation failed. {Failure}", clientId, validationResult.Failure);
            return new ErrorResult(requestData, validationResult.Failure!);
        }

        // create token
        var tokenResult = await grantTypeProvider.CreateTokenAsync(clientVerificationResult.Client, validationResult, requestData, cancellationToken);

        return new TokenResult(tokenResult);
    }
}
