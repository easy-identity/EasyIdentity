using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public class TokenStore : ITokenStore
{
    private static readonly ConcurrentDictionary<string, TokenDescriber> _tmp = new ConcurrentDictionary<string, TokenDescriber>();

    public virtual Task<TokenDescriber?> FindByIdAsync(Client client, string id, CancellationToken cancellationToken = default)
    {
        return _tmp.TryGetValue(id, out var token) ? Task.FromResult<TokenDescriber?>(token) : Task.FromResult<TokenDescriber?>(null);
    }

    public virtual Task SaveAsync(Client client, TokenDescriber token, CancellationToken cancellationToken = default)
    {
        _tmp.TryAdd(token.Id, token);

        return Task.CompletedTask;
    }
}