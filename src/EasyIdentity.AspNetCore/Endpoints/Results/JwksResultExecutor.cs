using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class JwksResultExecutor : EndpointResultExecutor<JwksResult>
{
    public override async Task ExecuteAsync(HttpContext context, JwksResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, new { keys = result.Keys }, cancellationToken: cancellationToken);
    }
}