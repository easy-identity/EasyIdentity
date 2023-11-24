using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints;

public abstract class EndpointResultExecutor<TResult> : IEndpointResultExecutor<TResult> where TResult : class, IEndpointResult
{
    public abstract Task ExecuteAsync(HttpContext context, TResult result, CancellationToken cancellationToken = default);

    protected virtual async Task WriteJsonDataAsync<T>(HttpContext context, T data, JsonSerializerOptions? jsonSerializerOptions = null, CancellationToken cancellationToken = default)
    {
        if (jsonSerializerOptions == null)
        {
            jsonSerializerOptions = context.RequestServices.GetRequiredService<IJsonSerializer>().GetOptions();
        }

        await context.Response.WriteAsJsonAsync(data, jsonSerializerOptions, cancellationToken: cancellationToken);
    }
}
