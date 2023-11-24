using EasyIdentity.Constants;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class TokenManager : ITokenManager
{
    protected ILogger<TokenManager> Logger { get; }
    protected ITokenGenerator TokenGenerator { get; }
    protected ITokenStore TokenStore { get; }
    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IIssuerUriProvider IssuerUriProvider { get; }
    protected IClientStore ClientStore { get; }

    public TokenManager(ITokenGenerator tokenGenerator, ITokenStore tokenStore, IOptions<EasyIdentityOptions> options, IIssuerUriProvider issuerUriProvider, ILogger<TokenManager> logger, IClientStore clientStore)
    {
        TokenGenerator = tokenGenerator;
        TokenStore = tokenStore;
        Options = options;
        IssuerUriProvider = issuerUriProvider;
        Logger = logger;
        ClientStore = clientStore;
    }

    public virtual async Task<TokenGenerated> GenerateAsync(Client client, ClaimsPrincipal claimsPrincipal, string[]? scopes, IRequestCollection requestData, bool identityToken = false, bool refreshToken = false, CancellationToken cancellationToken = default)
    {
        var token = await CreateTokenAsync(client, TokenConsts.AccessToken, scopes, claimsPrincipal, cancellationToken);

        var geneated = new TokenGenerated
        {
            AccessToken = await TokenGenerator.CreateSecurityTokenAsync(client, token, cancellationToken),
            AccessTokenLifetime = token.Lifetime,
            Scopes = scopes,
            TokenType = await TokenGenerator.GetTokenTypeAsync(client, token, cancellationToken),
        };

        if (identityToken)
        {
            token = await CreateTokenAsync(client, TokenConsts.IdentityToken, scopes, claimsPrincipal, cancellationToken);
            // token.AccessTokenHash = CryptoHelper.CreateHashClaimValue(geneated.AccessToken, "");
            geneated.IdentityToken = await TokenGenerator.CreateSecurityTokenAsync(client, token, cancellationToken);
        }

        if (refreshToken)
        {
            token = await CreateTokenAsync(client, TokenConsts.RefreshToken, scopes, claimsPrincipal, cancellationToken);
            // token.AccessTokenHash = CryptoHelper.CreateHashClaimValue(geneated.AccessToken, "");
            geneated.RefreshToken = await TokenGenerator.CreateRefreshTokenAsync(client, token, cancellationToken);
        }

        return geneated;
    }

    public virtual async Task<TokenDescriber> CreateTokenAsync(Client client, string tokenType, string[]? scopes, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        var subject = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var issuerUri = await IssuerUriProvider.GetAsync();

        var token = new TokenDescriber(Guid.NewGuid().ToString(), subject, client.Name)
        {
            Type = tokenType,
            Audience = client.Name,
            Claims = claimsPrincipal.Claims.ToList(),
            CreationTime = DateTime.UtcNow,
            Scopes = scopes,
            Issuer = issuerUri.ToString(),
        };

        if (tokenType == TokenConsts.AccessToken)
        {
            token.Lifetime = client.Token.AccessTokenLifetime ?? Options.Value.Token.DefaultAccessTokenLifetime;
        }
        else if (tokenType == TokenConsts.IdentityToken)
        {
            token.Lifetime = client.Token.IdTokenTokenLifetime ?? Options.Value.Token.DefaultIdentityTokenLifetime;
        }
        else if (tokenType == TokenConsts.RefreshToken)
        {
            token.Lifetime = client.Token.RefreshTokenLifetime ?? Options.Value.Token.DefaultRefreshTokenLifetime;
        }

        await TokenStore.SaveAsync(client, token);

        return token;
    }

    public virtual async Task<TokenValidationResult> ValidateAccessTokenAsync(Client client, string accessToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var jwtToken = new JsonWebToken(accessToken);

            var tokenId = jwtToken.Id;

            if (!jwtToken.Audiences.Contains(client.Name))
            {
                return TokenValidationResult.Error(Error.Create(Error.INVALID_GRANT));
            }
            var token = await TokenStore.FindByIdAsync(client, tokenId, cancellationToken);

            return token == null || token.Type != TokenConsts.AccessToken
                ? TokenValidationResult.Error(Error.Create(Error.INVALID_CLIENT))
                : new TokenValidationResult(token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Validate token failed.");
            return TokenValidationResult.Error(Error.Create(Error.SERVER_ERROR, ex.Message));
        }
    }

    public virtual async Task<TokenValidationResult> ValidateRefreshTokenAsync(Client client, string refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var jwtToken = new JsonWebToken(refreshToken);

            var tokenId = jwtToken.Id;

            if (!jwtToken.Audiences.Contains(client.Name))
            {
                return TokenValidationResult.Error(Error.Create(Error.INVALID_GRANT));
            }
            var token = await TokenStore.FindByIdAsync(client, tokenId, cancellationToken);

            return token == null || token.Type != TokenConsts.RefreshToken
                ? TokenValidationResult.Error(Error.Create(Error.INVALID_CLIENT))
                : new TokenValidationResult(token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Validate token failed.");
            return TokenValidationResult.Error(Error.Create(Error.SERVER_ERROR, ex.Message));
        }
    }

    public async Task<TokenValidationResult> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var jwtToken = new JsonWebToken(token);

            var tokenId = jwtToken.Id;
            var clientNames = jwtToken.Audiences;

            var client = await ClientStore.FindByNameAsync(clientNames.First());

            if (client == null)
            {
                TokenValidationResult.Error(Error.Create(Error.INVALID_CLIENT));
            }

            var tokenDescriber = await TokenStore.FindByIdAsync(client!, tokenId, cancellationToken);

            return (tokenDescriber == null)
                ? TokenValidationResult.Error(Error.Create(Error.INVALID_CLIENT))
                : new TokenValidationResult(tokenDescriber);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Validate token failed.");
            return TokenValidationResult.Error(Error.Create(Error.SERVER_ERROR, ex.Message));
        }
    }
}
