using EasyIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public interface ITokenStore
{
    Task<TokenDescriber?> FindByIdAsync(Client client, string id, CancellationToken cancellationToken = default);

    Task SaveAsync(Client client, TokenDescriber token, CancellationToken cancellationToken = default);
}
