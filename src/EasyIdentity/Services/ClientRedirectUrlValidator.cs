using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientRedirectUrlValidator : IClientRedirectUrlValidator
{
    public virtual Task<bool> ValidateAsync(Client client, string redirectUrl, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(client.RedirectUrls.Contains(redirectUrl));
    }
}
