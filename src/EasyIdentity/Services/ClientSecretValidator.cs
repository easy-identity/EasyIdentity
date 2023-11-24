using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientSecretValidator : IClientSecretValidator
{
    public virtual Task<bool> ValidateAsync(IClientSecret secret, string input, CancellationToken cancellationToken = default)
    {
        bool result = false;
        if (secret is ClientPlainSecret plainSecret)
        {
            result = plainSecret.Secret == input;
        }
        else if (secret is ClientHashedSecret hashedSecret)
        {
            result = ClientHashedSecret.Hash(input) == hashedSecret.Secret;
        }
        else
        {
            throw new NotSupportedException();
        }

        return Task.FromResult(result);
    }
}
