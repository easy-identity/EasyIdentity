using EasyIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public interface IClientStore
{
    Task<Client?> FindByNameAsync(string id, CancellationToken cancellationToken = default);
}
