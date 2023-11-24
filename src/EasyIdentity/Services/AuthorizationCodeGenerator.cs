using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationCodeGenerator : IAuthorizationCodeGenerator
{
    public virtual Task<string> GenerateAsync(Client client, string[]? scopes, string subject, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.NewGuid().ToString("N"));
    }
}
