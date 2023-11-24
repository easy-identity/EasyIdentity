using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientSecretValidator
{
    Task<bool> ValidateAsync(IClientSecret secret, string input, CancellationToken cancellationToken = default);
}