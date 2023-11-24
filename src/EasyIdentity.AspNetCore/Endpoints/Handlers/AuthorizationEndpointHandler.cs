using EasyIdentity.Constants;
using EasyIdentity.Endpoints.Results;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/connect/authorize", Method = "GET")]
public class AuthorizationEndpointHandler : EndpointHandler
{
    public override string Name => "authorize";

    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IClientRequestValidator ClientRequestValidator { get; }
    protected IAuthorizationService AuthorizationService { get; }
    protected IAuthorizationCodeManager AuthorizationCodeManager { get; }
    protected ITokenManager TokenManager { get; }
    protected IGrantTypeTokenProviderFactory GrantTypeProviderFactory { get; }

    public AuthorizationEndpointHandler(
        IOptions<EasyIdentityOptions> options,
        IClientRequestValidator clientValidator,
        IAuthorizationService authorizeInteractionProvider,
        IAuthorizationCodeManager authorizationCodeManager,
        ITokenManager tokenManager,
        IGrantTypeTokenProviderFactory grantTypeProviderFactory)
    {
        Options = options;
        ClientRequestValidator = clientValidator;
        AuthorizationService = authorizeInteractionProvider;
        AuthorizationCodeManager = authorizationCodeManager;
        TokenManager = tokenManager;
        GrantTypeProviderFactory = grantTypeProviderFactory;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var clientId = requestData.ClientId;
        var responseTypes = requestData.ResponseTypes;

        var clientValidationResult = await ClientRequestValidator.ValidateAsync(Name, clientId!, requestData, cancellationToken);

        if (!clientValidationResult.Succeeded)
        {
            return await HandleClientValidationFailureAsync(clientValidationResult, requestData);
        }

        var result = await HttpContext.AuthenticateAsync(Options.Value.Authentication.AuthenticationScheme);

        if (result.Succeeded)
        {
            var authorizeResult = await AuthorizationService.AuthorizeAsync(clientValidationResult.Client, requestData, cancellationToken);
            if (!authorizeResult.IsGranted)
            {
                return new ConsentResult(Options.Value.Authentication.ConsentUrl);
            }

            if (responseTypes.Length == 1 && responseTypes[0] == "code")
            {
                return await AuthorizationCodeFlowAsync(clientValidationResult.Client, result.Principal, authorizeResult, requestData, cancellationToken);
            }
            else if (responseTypes.Contains("token") || responseTypes.Contains("id_token"))
            {
                return await ImplicitFlowAsync(clientValidationResult.Client, result.Principal, authorizeResult, requestData, cancellationToken);
            }

            return new ErrorResult(requestData, Error.Create("invaid_client"));
        }

        return new ChallengeResult(Options.Value.Authentication.AuthenticationScheme);
    }

    protected virtual Task<ErrorResult> HandleClientValidationFailureAsync(ClientValidationResult result, IRequestCollection requestData)
    {
        return Task.FromResult(new ErrorResult(requestData, result.Failure));
    }

    protected virtual async Task<IEndpointResult> AuthorizationCodeFlowAsync(Client client, ClaimsPrincipal claimsPrincipal, AuthorizeResult authorizeResult, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var codeChallenge = requestData.CodeChallenge;
        var codeChallengeMethod = requestData.CodeChallengeMethod;

        var code = await AuthorizationCodeManager.GenerateAsync(client, claimsPrincipal, authorizeResult.Scopes, requestData.RedirectUri!, codeChallengeMethod, codeChallenge, cancellationToken);

        return new AuthorizationCodeResult(requestData, code);
    }

    protected virtual async Task<IEndpointResult> ImplicitFlowAsync(Client client, ClaimsPrincipal claimsPrincipal, AuthorizeResult authorizeResult, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var subject = claimsPrincipal.GetSubject();
        var scopes = requestData.Scopes;

        // create token
        var provider = GrantTypeProviderFactory.Get(GrantTypes.Implicit);
        var tokenResult = await provider.CreateTokenAsync(client, new TokenRequestValidationResult(subject, scopes), requestData, cancellationToken);

        string? code = null;
        if (requestData.ResponseTypes.Contains("code"))
        {
            code = await AuthorizationCodeManager.GenerateAsync(client, claimsPrincipal, authorizeResult.Scopes, requestData.RedirectUri!, cancellationToken: cancellationToken);
        }

        return new ImplicitTokenResult(requestData, tokenResult, code);
    }
}
