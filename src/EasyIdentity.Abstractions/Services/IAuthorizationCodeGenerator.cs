using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeGenerator
{
    Task<string> GenerateAsync(Client client, string[]? scopes, string subject, CancellationToken cancellationToken = default);
}