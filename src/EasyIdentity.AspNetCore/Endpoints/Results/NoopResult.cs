using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class NoopResult : IEndpointResult
{
    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<NoopResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
