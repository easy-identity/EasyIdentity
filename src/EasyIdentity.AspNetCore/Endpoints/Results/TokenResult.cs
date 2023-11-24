using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class TokenResult : IEndpointResult
{
    public TokenGenerated Token { get; }

    public TokenResult(TokenGenerated token)
    {
        Token = token;
    }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<TokenResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
