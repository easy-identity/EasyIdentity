using EasyIdentity.Constants;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class AuthorizationCodeManager : IAuthorizationCodeManager
{
    protected ILogger<AuthorizationCodeManager> Logger { get; }
    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IAuthorizationCodeStore AuthorizationCodeStore { get; }
    protected IAuthorizationCodeGenerator AuthorizationCodeGenerator { get; }

    public AuthorizationCodeManager(ILogger<AuthorizationCodeManager> logger, IAuthorizationCodeStore authorizationCodeStore, IAuthorizationCodeGenerator authorizationCodeGenerator, IOptions<EasyIdentityOptions> options)
    {
        Logger = logger;
        AuthorizationCodeStore = authorizationCodeStore;
        AuthorizationCodeGenerator = authorizationCodeGenerator;
        Options = options;
    }

    public virtual async Task<string> FindSubjectAsync(Client client, string code, CancellationToken cancellationToken = default)
    {
        var authorizationCode = await AuthorizationCodeStore.FindByCodeAsync(code, cancellationToken);

        return authorizationCode == null
            ? throw new InvalidOperationException("AuthorizationCode must not be null.")
            : authorizationCode.Subject;
    }

    [Obsolete]
    public virtual async Task<string> GenerateAsync(Client client, ClaimsPrincipal claimsPrincipal, string[]? scopes, string redirectUri, string? codeChallengeMethod = null, string? codeChallenge = null, CancellationToken cancellationToken = default)
    {
        var code = await AuthorizationCodeGenerator.GenerateAsync(client, scopes, claimsPrincipal.GetSubject(), cancellationToken);

        await AuthorizationCodeStore.SaveAsync(new AuthorizationCode()
        {
            ClientId = client.Name,
            Code = code,
            CreationTime = DateTimeOffset.UtcNow,
            Lifetime = client.Token.AccessTokenLifetime ?? Options.Value.Token.DefaultAuthorizationCodeLifetime,
            RedirectUri = redirectUri,
            Scopes = scopes,
            Subject = claimsPrincipal.GetSubject(),
            CodeChallengeMethod = codeChallengeMethod,
            CodeChallenge = codeChallenge,
        }, cancellationToken);

        return code;
    }

    public async Task<bool> ValidateCodeAsync(Client client, string code, string? verifier, string[]? scopes, string? redirectUri, CancellationToken cancellationToken = default)
    {
        var authorizationCode = await AuthorizationCodeStore.FindByCodeAsync(code, cancellationToken);

        if (authorizationCode == null)
        {
            return false;
        }

        if (authorizationCode.ClientId != client.Name)
        {
            return false;
        }

        if (authorizationCode.Scopes != null && scopes?.Except(authorizationCode.Scopes).Any() == true)
        {
            return false;
        }

        if (authorizationCode.RedirectUri != redirectUri)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(authorizationCode.CodeChallengeMethod))
        {
            var codeChallenge = authorizationCode.CodeChallenge;

            if (authorizationCode.CodeChallengeMethod == Consts.CodeChallengeMethods.Plain)
            {
                if (codeChallenge != verifier)
                {
                    return false;
                }
            }
            else if (authorizationCode.CodeChallengeMethod == Consts.CodeChallengeMethods.Sha256)
            {
                using var sha = SHA256.Create();

                var targetValue = Base64Helper.Encode(sha.ComputeHash(Encoding.ASCII.GetBytes(verifier)));
                if (targetValue != codeChallenge)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<string[]> GetScopesAsync(Client client, string code, CancellationToken cancellationToken = default)
    {
        var authorizationCode = await AuthorizationCodeStore.FindByCodeAsync(code, cancellationToken);

        return authorizationCode?.Scopes ?? Array.Empty<string>();
    }
}
