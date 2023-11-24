using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.SecurityKeys;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class JwtTokenGenerator : ITokenGenerator
{
    protected IJsonSerializer JsonSerializer { get; }
    protected ICredentialsManager CredentialsManager { get; }
    protected IOptions<EasyIdentityOptions> Options { get; }

    public JwtTokenGenerator(IJsonSerializer jsonSerializer, ICredentialsManager credentialsManager, IOptions<EasyIdentityOptions> options)
    {
        JsonSerializer = jsonSerializer;
        CredentialsManager = credentialsManager;
        Options = options;
    }

    public virtual async Task<string> CreateSecurityTokenAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default)
    {
        var securityTokenDescriptor = CreateSecurityToken(tokenDescriber);

        return await CreateToken(securityTokenDescriptor);
    }

    public virtual async Task<string> CreateRefreshTokenAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default)
    {
        var headers = new Dictionary<string, object>();

        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, tokenDescriber.Subject) });

        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            AdditionalHeaderClaims = headers,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(Options.Value.Token.DefaultRefreshTokenLifetime),
            NotBefore = tokenDescriber.CreationTime.UtcDateTime,
            Claims = new Dictionary<string, object>() { { JwtRegisteredClaimNames.Jti, tokenDescriber.Id } },
            Subject = claimsIdentity,
            TokenType = "Bearer",
            Issuer = tokenDescriber.Issuer,
            Audience = tokenDescriber.Audience,
        };

        return await CreateToken(securityTokenDescriptor);
    }

    protected SecurityTokenDescriptor CreateSecurityToken(TokenDescriber tokenDescriber)
    {
        var headers = new Dictionary<string, object>();

        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, tokenDescriber.Subject) });

        var claims = tokenDescriber.Claims.ToDictionary(x => x.Type, x => (object)x.Value);

        claims.Add(JwtRegisteredClaimNames.Jti, tokenDescriber.Id);
        claims.Add(JwtRegisteredClaimNames.Sub, tokenDescriber.Subject);
        //claims.Add(JwtRegisteredClaimNames.Azp, tokenDescriber.ClientId);

        if (tokenDescriber.Scopes != null)
        {
            claims.Add("scope", string.Join(" ", tokenDescriber.Scopes));
        }

        if (!string.IsNullOrWhiteSpace(tokenDescriber.AccessTokenHash))
        {
            claims.Add(JwtRegisteredClaimNames.AtHash, tokenDescriber.AccessTokenHash!);
        }
        if (!string.IsNullOrWhiteSpace(tokenDescriber.AuthorizationCodeHash))
        {
            claims.Add(JwtRegisteredClaimNames.CHash, tokenDescriber.AuthorizationCodeHash!);
        }

        return new SecurityTokenDescriptor()
        {
            AdditionalHeaderClaims = headers,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(Options.Value.Token.DefaultRefreshTokenLifetime),
            NotBefore = tokenDescriber.CreationTime.UtcDateTime,
            Claims = claims,
            Subject = claimsIdentity,
            Issuer = tokenDescriber.Issuer,
            Audience = tokenDescriber.Audience,
        };
    }

    protected async Task<string> CreateToken(SecurityTokenDescriptor securityTokenDescriptor)
    {
        var handler = new JwtSecurityTokenHandler();

        securityTokenDescriptor.SigningCredentials = await GetSigningCredentials();

        return handler.CreateEncodedJwt(securityTokenDescriptor);
    }

    protected async Task<SigningCredentials> GetSigningCredentials()
    {
        var credentials = await CredentialsManager.GetSigningCredentialsAsync();

        var credential = credentials[0];

        if (credential is RsaSigningSecurityKey rsaSigningSecurityKey)
        {
            return new SigningCredentials(new RsaSecurityKey(rsaSigningSecurityKey.Rsa), rsaSigningSecurityKey.Algorithm);
        }
        else if (credential is X509SigningSecurityKey x509SigningSecurityKey)
        {
            return new X509SigningCredentials(x509SigningSecurityKey.Certificate, x509SigningSecurityKey.Algorithm);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public virtual Task<string> GetTokenTypeAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default)
    {
        return Task.FromResult("Bearer");
    }
}
