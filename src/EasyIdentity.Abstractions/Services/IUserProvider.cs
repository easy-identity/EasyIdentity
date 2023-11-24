using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IUserProvider
{
    Task<List<Claim>> GetClaimsAsync(string subject, string []? scopes, CancellationToken cancellationToken = default);

    Task<UserIdentityVerificationResult> ValidatePasswordAsync(string userName, string? password, IRequestCollection requestData, CancellationToken cancellationToken = default);
}
