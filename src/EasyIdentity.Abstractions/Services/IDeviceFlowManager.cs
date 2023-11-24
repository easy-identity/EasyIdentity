using EasyIdentity.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IDeviceFlowManager
{
    Task<string> GenerateDeviceCodeAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default);

    Task<string> GenerateUserCodeAsync(Client client, string deviceCode, IRequestCollection requestData, CancellationToken cancellationToken = default);

    Task SetUserAuthorizedAsync(string userCode, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);

    Task SetUserRejectdAsync(string userCode, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);

    Task<bool> IsUserCodeValidAsync(string userCode, CancellationToken cancellationToken = default);

    Task<DeviceCodeValidationResult> ValidateDeviceCodeAsync(Client client, string deviceCode, IRequestCollection requestData, CancellationToken cancellationToken = default);

    Task<string[]> GetScopesByDeviceCodeAsync(Client client, string deviceCode, CancellationToken cancellationToken = default);
}
