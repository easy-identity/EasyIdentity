using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class RedirectResultExecutor : IEndpointResultExecutor<RedirectResult>
{
    public Task ExecuteAsync(HttpContext context, RedirectResult result, CancellationToken cancellationToken = default)
    {
        context.Response.Redirect(result.RedirectUrl, false);

        return Task.CompletedTask;
    }
}
