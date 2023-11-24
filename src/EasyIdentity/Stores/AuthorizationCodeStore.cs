using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public class AuthorizationCodeStore : IAuthorizationCodeStore
{
    private static readonly ConcurrentDictionary<string, AuthorizationCode> Tmp = new ConcurrentDictionary<string, AuthorizationCode>();

    public virtual Task<AuthorizationCode?>? FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (Tmp.TryGetValue(code, out var result))
        {
            return Task.FromResult(result);
        }

        return Task.FromResult<AuthorizationCode?>(null);
    }

    public virtual Task SaveAsync(AuthorizationCode code, CancellationToken cancellationToken = default)
    {
        Tmp[code.Code] = code;
        return Task.CompletedTask;
    }
}
