using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public interface IEndpointResult
{
    Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default);
}
