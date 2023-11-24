using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Endpoints.Results;

public class ImplicitTokenResult : IEndpointResult
{
    public ImplicitTokenResult(IRequestCollection request, TokenGenerated token, string? code)
    {
        Request = request;
        Token = token;
        Code = code;
    }

    public IRequestCollection Request { get; }
    public TokenGenerated Token { get; }
    public string? Code { get; }

    public async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<ImplicitTokenResult>>();
        await executor.ExecuteAsync(context, this, cancellationToken);
    }
}
