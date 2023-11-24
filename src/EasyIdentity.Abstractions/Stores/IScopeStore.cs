using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public interface IScopeStore
{
    Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
}