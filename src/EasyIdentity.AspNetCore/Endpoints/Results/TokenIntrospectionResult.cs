using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Results;

public class TokenIntrospectionResult : IEndpointResult
{
    public TokenIntrospectionData Data { get; }

    public TokenIntrospectionResult(TokenIntrospectionData data)
    {
        Data = data;
    }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<TokenIntrospectionResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}