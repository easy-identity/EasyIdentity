using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientManager
{
    Task<Client?> FindByIdAsync(string clientId, CancellationToken cancellationToken = default);
    Task<bool> CheckSecretAsync(Client client, string? clientSecret, CancellationToken cancellationToken = default);
}
