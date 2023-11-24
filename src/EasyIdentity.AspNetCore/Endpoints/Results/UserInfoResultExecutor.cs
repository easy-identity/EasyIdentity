using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class UserInfoResultExecutor : EndpointResultExecutor<UserInfoResult>
{
    public override async Task ExecuteAsync(HttpContext context, UserInfoResult result, CancellationToken cancellationToken = default)
    {
        await WriteJsonDataAsync(context, result.Claims.ToDictionary(x => x.Type, x => x.Value), cancellationToken: cancellationToken);
    }
}
