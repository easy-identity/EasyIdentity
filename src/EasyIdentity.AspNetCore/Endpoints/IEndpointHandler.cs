using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public interface IEndpointHandler
{
    string Name { get; }

    Task<IEndpointResult> ProcessRequestAsync(HttpContext context, CancellationToken cancellationToken = default);
}
