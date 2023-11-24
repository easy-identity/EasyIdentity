using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class RedirectResult : IEndpointResult
{
    public string RedirectUrl { get; }

    public RedirectResult(string redirectUrl)
    {
        RedirectUrl = redirectUrl;
    }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<RedirectResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
