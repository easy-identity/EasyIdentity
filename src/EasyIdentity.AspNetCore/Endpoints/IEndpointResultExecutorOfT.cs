using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public interface IEndpointResultExecutor<in TResult> where TResult : IEndpointResult
{
    Task ExecuteAsync(HttpContext context, TResult result, CancellationToken cancellationToken = default);
}
