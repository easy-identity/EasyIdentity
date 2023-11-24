using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface ITokenGenerator
{
    Task<string> GetTokenTypeAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default);

    Task<string> CreateSecurityTokenAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default);

    Task<string> CreateRefreshTokenAsync(Client client, TokenDescriber tokenDescriber, CancellationToken cancellationToken = default);
}