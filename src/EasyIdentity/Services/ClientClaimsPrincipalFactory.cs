using EasyIdentity.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class ClientClaimsPrincipalFactory : IClientClaimsPrincipalFactory
{
    public virtual Task<ClaimsPrincipal> CreateAsync(Client client, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, client.Name),
            new Claim(ClaimTypes.Name, client.Name)
        };

        return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(claims)));
    }
}
