using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class ChallengeResultExecutor : IEndpointResultExecutor<ChallengeResult>
{
    public async Task ExecuteAsync(HttpContext context, ChallengeResult result, CancellationToken cancellationToken = default)
    {
        await context.ChallengeAsync(result.Schame);
    }
}
