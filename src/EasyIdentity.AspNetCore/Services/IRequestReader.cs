using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public interface IRequestReader
{
    ValueTask<IRequestCollection> ReadAsync(HttpContext httpContext, CancellationToken cancellationToken = default);
}
