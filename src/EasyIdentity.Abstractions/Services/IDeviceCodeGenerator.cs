using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IDeviceCodeGenerator
{
    Task<string> GenerateUserCodeAsync(Client client, string[]? scopes, CancellationToken cancellationToken = default);
    Task<string> GenerateDeviceCodeAsync(Client client, string[]? scopes, CancellationToken cancellationToken = default);
}