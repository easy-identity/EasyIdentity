using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IInteractionService
{
    Task<DeviceFlowAuthorizedResult> DeviceAuthorizeAsync(string userCode, ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
