using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Results;

public class TokenIntrospectionResultExecutor : EndpointResultExecutor<TokenIntrospectionResult>
{
    public override async Task ExecuteAsync(HttpContext context, TokenIntrospectionResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, result.Data, cancellationToken: cancellationToken);
    }
}
