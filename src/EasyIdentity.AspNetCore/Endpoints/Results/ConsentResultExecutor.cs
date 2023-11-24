using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class ConsentResultExecutor : IEndpointResultExecutor<ConsentResult>
{
    public Task ExecuteAsync(HttpContext context, ConsentResult result, CancellationToken cancellationToken = default)
    {
        context.Response.Redirect(result.RedirectUrl, false);

        return Task.CompletedTask;
    }
}
