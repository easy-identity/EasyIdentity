using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/connect/userinfo", Method = "GET")]
public class UserinfoEndpointHanlder : EndpointHandler
{
    public override string Name => "userinfo";

    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IUserProvider UserIdentityProvider { get; }
    protected ITokenManager TokenManager { get; }
    protected IClientRequestValidator ClientRequestValidator { get; }

    public UserinfoEndpointHanlder(IOptions<EasyIdentityOptions> options, IUserProvider userIdentityProvider, ITokenManager tokenManager, IClientRequestValidator clientRequestValidator)
    {
        Options = options;
        UserIdentityProvider = userIdentityProvider;
        TokenManager = tokenManager;
        ClientRequestValidator = clientRequestValidator;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        if (!requestData.Authorization.IsBearerAuth())
        {
            return new UnauthenticatedResult();
        }

        var token = requestData.Authorization.Parameter!;

        if (string.IsNullOrWhiteSpace(token))
        {
            return new UnauthenticatedResult();
        }

        var validationResult = await TokenManager.ValidateTokenAsync(token, cancellationToken);

        if (validationResult.Succeeded)
        {
            var claims = await UserIdentityProvider.GetClaimsAsync(validationResult.Token.Subject, null, cancellationToken);

            return new UserInfoResult(claims);
        }

        return new ErrorResult(requestData, Error.Create(Error.INVALID_REQUEST));
    }
}
