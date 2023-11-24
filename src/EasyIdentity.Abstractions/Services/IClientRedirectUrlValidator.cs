using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientRedirectUrlValidator
{
    Task<bool> ValidateAsync(Client client, string? redirectUrl, CancellationToken cancellationToken = default);
}
