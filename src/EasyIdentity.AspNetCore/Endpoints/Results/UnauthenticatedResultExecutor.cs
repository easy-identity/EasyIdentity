using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class UnauthenticatedResultExecutor : IEndpointResultExecutor<UnauthenticatedResult>
{
    public Task ExecuteAsync(HttpContext context, UnauthenticatedResult result, CancellationToken cancellationToken = default)
    {
        context.Response.StatusCode = 401;

        return Task.CompletedTask;
    }
}
