using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class DeviceCodeGenerator : IDeviceCodeGenerator
{
#if NETSTANDARD
    private static readonly Random Random = new();
#endif

    public virtual Task<string> GenerateDeviceCodeAsync(Client client, string[]? scopes, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }

    public virtual Task<string> GenerateUserCodeAsync(Client client, string[]? scopes, CancellationToken cancellationToken = default)
    {
#if NETSTANDARD
        var value = Random.Next(1000, 9999);
#else
        var value = RandomNumberGenerator.GetInt32(1000, 9999);
#endif
        return Task.FromResult(value.ToString());
    }
}
