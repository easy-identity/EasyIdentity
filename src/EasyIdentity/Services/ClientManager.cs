using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class ClientManager : IClientManager
{
    protected ILogger<ClientManager> Logger { get; }
    protected IClientStore ClientStore { get; }
    protected IClientSecretValidator ClientCredentialValidator { get; }

    public ClientManager(ILogger<ClientManager> logger, IClientStore clientStore, IClientSecretValidator clientCredentialValidator)
    {
        Logger = logger;
        ClientStore = clientStore;
        ClientCredentialValidator = clientCredentialValidator;
    }

    public virtual async Task<bool> CheckSecretAsync(Client client, string clientSecret, CancellationToken cancellationToken = default)
    {
        if (client.Serets.Any())
        {
            foreach (var item in client.Serets)
            {
                if (await ClientCredentialValidator.ValidateAsync(item, clientSecret, cancellationToken))
                {
                    return true;
                }
            }
        }

        return true;
    }

    public virtual async Task<Client?> FindByIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await ClientStore.FindByNameAsync(clientId, cancellationToken);
    }
}
