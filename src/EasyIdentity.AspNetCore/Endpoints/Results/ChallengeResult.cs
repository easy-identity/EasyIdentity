using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class ChallengeResult : IEndpointResult
{
    public string Schame { get; }

    public ChallengeResult(string schame)
    {
        Schame = schame;
    }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<ChallengeResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
