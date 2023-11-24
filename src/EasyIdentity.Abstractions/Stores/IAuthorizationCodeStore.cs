using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public interface IAuthorizationCodeStore
{
    Task<AuthorizationCode?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task SaveAsync(AuthorizationCode code, CancellationToken cancellationToken = default);
}
