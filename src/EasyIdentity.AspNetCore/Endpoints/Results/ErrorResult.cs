using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class ErrorResult : IEndpointResult
{
    public ErrorResult(IRequestCollection requestData, Error error)
    {
        RequestData = requestData;
        Data = error;
    }

    public IRequestCollection RequestData { get; }
    public Error Data { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<ErrorResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
