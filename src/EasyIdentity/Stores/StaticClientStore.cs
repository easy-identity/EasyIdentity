using EasyIdentity.Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public class StaticClientStore : IClientStore
{
    private static ConcurrentDictionary<string, Client> _cache = new ConcurrentDictionary<string, Client>();

    public StaticClientStore(params Client[] clients)
    {
        var clientIds = clients.Select(x => x.Name).ToArray();
        _cache = new ConcurrentDictionary<string, Client>(clientIds.ToDictionary(x => x, x => clients.First(c => c.Name == x)));
    }

    public virtual Task<Client?> FindByNameAsync(string id, CancellationToken cancellationToken = default)
    {
        return _cache.TryGetValue(id, out var result) ? Task.FromResult<Client?>(result) : Task.FromResult<Client?>(null);
    }
}
