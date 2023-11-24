using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public class ClientStore : IClientStore
{
    public Task<Client?> FindByNameAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetAllGrantTypesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetAllResponseTypesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
