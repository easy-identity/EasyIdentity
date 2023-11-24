using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class NullUserProvider : IUserProvider
{
    protected ILogger<NullUserProvider> Logger { get; }

    public NullUserProvider(ILogger<NullUserProvider> logger)
    {
        Logger = logger;
    }

    public virtual Task<List<Claim>> GetClaimsAsync(string subject, string[]? scopes, CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("PLEASE IMPLEMENT '{0}'", typeof(IUserProvider).FullName);
        return Task.FromResult(new List<Claim>());
    }

    public virtual Task<UserIdentityVerificationResult> ValidatePasswordAsync(string userName, string? password, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("PLEASE IMPLEMENT '{0}'", typeof(IUserProvider).FullName);

        return Task.FromResult(UserIdentityVerificationResult.Error(new NotImplementedException()));
    }
}
