using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class AuthorizationCodeResult : IEndpointResult
{
    public IRequestCollection Request { get; }
    public string Code { get; }

    public AuthorizationCodeResult(IRequestCollection request, string code)
    {
        Request = request;
        Code = code;
    }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<AuthorizationCodeResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
