using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientRequestValidator
{
    Task<ClientValidationResult> ValidateAsync(string endpoint, string clientId, IRequestCollection requestData, CancellationToken cancellationToken = default);
}
