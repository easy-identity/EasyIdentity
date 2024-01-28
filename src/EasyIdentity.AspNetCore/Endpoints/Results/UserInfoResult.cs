using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Endpoints.Results;

public class UserInfoResult : IEndpointResult
{
    public UserInfoResult(IEnumerable<Claim> claims)
    {
        Claims = claims;
    }

    public IEnumerable<Claim> Claims { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<UserInfoResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
