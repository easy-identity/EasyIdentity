using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#if !NET8_0_OR_GREATER
using EasyIdentity.Json;
#endif

namespace EasyIdentity.Endpoints.Results;

public class DeviceAuthorizationResultExecutor : EndpointResultExecutor<DeviceAuthorizationResult>
{
    public override async Task ExecuteAsync(HttpContext context, DeviceAuthorizationResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, result.Data, cancellationToken: cancellationToken);
    }
}
