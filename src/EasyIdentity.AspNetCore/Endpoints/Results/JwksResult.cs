using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class JwksResult : IEndpointResult
{
    public JwksResult(IEnumerable<JwkKeyInfo> jwkKeys)
    {
        Keys = jwkKeys;
    }

    public IEnumerable<JwkKeyInfo> Keys { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<JwksResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
