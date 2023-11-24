using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class DeviceAuthorizationResult : IEndpointResult
{
    public DeviceAuthorizationResult(DeviceAuthorizationResultData data)
    {
        Data = data;
    }

    public DeviceAuthorizationResultData Data { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<DeviceAuthorizationResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
