using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AlwaysGrantedAuthorizationService : IAuthorizationService
{
    public virtual Task<AuthorizeResult> AuthorizeAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AuthorizeResult() { IsGranted = true, Scopes = requestData.Scopes });
    }
}
