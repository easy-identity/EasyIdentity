using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.SecurityKeys;

namespace EasyIdentity.Services;

public interface ICredentialsManager
{
    Task<List<SigningSecurityKey>> GetSigningCredentialsAsync(CancellationToken cancellationToken = default);

    Task<List<EncryptingSecurityKey>> GetEncryptingCredentialsAsync(CancellationToken cancellationToken = default);
}
