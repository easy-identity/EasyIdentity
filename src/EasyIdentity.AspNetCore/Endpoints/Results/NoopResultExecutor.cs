using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class NoopResultExecutor : IEndpointResultExecutor<NoopResult>
{
    public Task ExecuteAsync(HttpContext context, NoopResult result, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}