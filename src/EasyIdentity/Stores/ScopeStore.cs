using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public class StaticScopeStore : IScopeStore
{
    private static ConcurrentBag<string> s_scopes = [];

    public StaticScopeStore(params string[] scopes)
    {
        s_scopes = new ConcurrentBag<string>(scopes.ToArray());
    }

    public virtual Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<string>());
    }
}
