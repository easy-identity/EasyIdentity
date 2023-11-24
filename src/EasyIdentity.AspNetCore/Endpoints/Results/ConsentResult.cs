using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class ConsentResult : RedirectResult
{
    public ConsentResult(string redirectUrl) : base(redirectUrl)
    {
    }

    public new async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<ConsentResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
