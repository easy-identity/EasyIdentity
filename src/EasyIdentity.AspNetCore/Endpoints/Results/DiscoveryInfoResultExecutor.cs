using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#if !NET8_0_OR_GREATER
using EasyIdentity.Json;
#endif

namespace EasyIdentity.Endpoints.Results;

public class DiscoveryInfoResultExecutor : EndpointResultExecutor<DiscoveryInfoResult>
{
    public override async Task ExecuteAsync(HttpContext context, DiscoveryInfoResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, result.Data, cancellationToken: cancellationToken);
    }
}