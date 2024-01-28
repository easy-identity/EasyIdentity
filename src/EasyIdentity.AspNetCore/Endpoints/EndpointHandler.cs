using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints;

public abstract class EndpointHandler : IEndpointHandler
{
    public abstract string Name { get; }

    protected HttpContext HttpContext { get; private set; } = null!;

    protected IRequestReader RequestReader => HttpContext.RequestServices.GetRequiredService<IRequestReader>();
    protected ILogger Logger => (HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()).CreateLogger(GetType());

    protected EndpointHandler()
    {
    }

    public async Task<IEndpointResult> ProcessRequestAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        HttpContext = context;

        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(GetType());

        logger.LogDebug("Handle {Name} endpoint request ", Name);

        var requestData = await RequestReader.ReadAsync(context, cancellationToken);

        try
        {
            return await HandleAsync(requestData, cancellationToken);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Handle {Name} endpoint request failed", Name);
            return new ErrorResult(requestData, Error.Create("server_error"));
        }
    }

    public abstract Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default);
}