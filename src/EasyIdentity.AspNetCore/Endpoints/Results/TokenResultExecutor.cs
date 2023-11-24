using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class TokenResultExecutor : EndpointResultExecutor<TokenResult>
{
    public override async Task ExecuteAsync(HttpContext context, TokenResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, new
        {
            access_token = result.Token.AccessToken,
            token_type = result.Token.TokenType,
            expires_in = result.Token.AccessTokenLifetime.TotalSeconds,
            scope = result.Token.Scopes == null ? null : string.Join(" ", result.Token.Scopes),
            refresh_token = result.Token.RefreshToken,
            id_token = result.Token.IdentityToken,
        }, cancellationToken: cancellationToken);
    }
}