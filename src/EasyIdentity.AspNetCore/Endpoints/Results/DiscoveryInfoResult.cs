using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class DiscoveryInfoResult : IEndpointResult
{
    public DiscoveryInfoResult(DiscoveryInfoModel model)
    {
        Data = model;
    }

    public DiscoveryInfoModel Data { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<DiscoveryInfoResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
