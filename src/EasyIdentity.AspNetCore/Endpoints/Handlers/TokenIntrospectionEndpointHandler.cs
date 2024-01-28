using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Services;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/connect/introspect", Method = "POST")]
public class TokenIntrospectionEndpointHandler : EndpointHandler
{
    public override string Name => "introspect";

    protected ITokenManager TokenManager { get; }
    protected IClientRequestValidator ClientRequestValidator { get; }

    public TokenIntrospectionEndpointHandler(ITokenManager tokenManager, IClientRequestValidator clientRequestValidator)
    {
        TokenManager = tokenManager;
        ClientRequestValidator = clientRequestValidator;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        if (!HttpContext.Request.HasFormContentType)
        {
            return new ErrorResult(requestData, Error.Create(Error.INVALID_REQUEST));
        }

        var token = requestData["token"];

        var resultData = new TokenIntrospectionData() { Active = false };

        if (!string.IsNullOrWhiteSpace(token))
        {
            var validationResult = await TokenManager.ValidateTokenAsync(token!, cancellationToken);

            if (validationResult.Succeeded)
            {
                resultData = new TokenIntrospectionData
                {
                    Active = true,
                    ClientId = validationResult.Token.ClientId,
                    Scope = validationResult.Token == null ? string.Empty : string.Join(" ", validationResult.Token.Scopes!),
                    ExpiresIn = (int)validationResult.Token!.Lifetime.TotalSeconds,
                    Username = validationResult.Token.Subject,
                };
            }
        }

        return new TokenIntrospectionResult(resultData);
    }
}
