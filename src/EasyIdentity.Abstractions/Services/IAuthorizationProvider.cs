using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationService
{
    Task<AuthorizeResult> AuthorizeAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default);
}
