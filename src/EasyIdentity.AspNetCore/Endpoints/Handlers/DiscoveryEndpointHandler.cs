using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/.well-known/openid-configuration")]
public class DiscoveryEndpointHandler : EndpointHandler
{
    public override string Name => "discovery";

    protected EasyIdentityOptions Options { get; }
    protected IScopeStore ScopeStore { get; }
    protected IClientStore ClientStore { get; }
    protected IIssuerUriProvider IssuerUriProvider { get; }

    public DiscoveryEndpointHandler(
        IOptions<EasyIdentityOptions> options,
        IScopeStore scopeStore,
        IClientStore clientStore,
        IIssuerUriProvider issuerUriProvider)
    {
        Options = options.Value;
        ScopeStore = scopeStore;
        ClientStore = clientStore;
        IssuerUriProvider = issuerUriProvider;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var request = HttpContext.Request;

        var issuerUri = (await IssuerUriProvider.GetAsync());

        var issuerUrl = issuerUri.ToString();

        issuerUrl = issuerUrl.Substring(0, issuerUrl.Length - 1);

        var grantTypes = GrantTypes.All;
        var responseTypes = ResponseTypes.All;

        var scopes = await ScopeStore.GetAllAsync(cancellationToken);
        scopes.AddRange(Consts.AllStandardScopes);

        var model = new DiscoveryInfoModel()
        {
            Issuer = issuerUrl + "/",
            EndSessionEndpoint = issuerUrl + Options.Endpoint.EndSessionPath,
            AuthorizationEndpoint = issuerUrl + Options.Endpoint.AuthorizationPath,
            DeviceAuthorizationEndpoint = issuerUrl + Options.Endpoint.DeviceAuthorizationPath,
            IntrospectionEndpoint = issuerUrl + Options.Endpoint.IntrospectionPath,
            RevocationEndpoint = issuerUrl + Options.Endpoint.RevocationPath,
            TokenEndpoint = issuerUrl + Options.Endpoint.TokenPath,
            UserinfoEndpoint = issuerUrl + Options.Endpoint.UserinfoPath,
            JwksUri = issuerUrl + Options.Endpoint.JwksPath,
            ClaimsSupported = [],

            ScopesSupported = scopes.Distinct().ToArray(),
            GrantTypesSupported = grantTypes.ToArray(),
            ResponseTypesSupported = responseTypes.ToArray(),
            ResponseModesSupported = Consts.ResponseModesSupporteds.ToArray(),
            CodeChallengeMethodsSupported = Consts.SupportedCodeChallengeMethods.ToArray(),
            SubjectTypesSupported = new[] { "public" },

            RequestParameterSupported = true,
        };

        return new DiscoveryInfoResult(model);
    }
}
