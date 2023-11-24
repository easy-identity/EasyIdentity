using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class InteractionService : IInteractionService
{
    private readonly IDeviceFlowManager _deviceCodeManager;

    public InteractionService(IDeviceFlowManager deviceCodeManager)
    {
        _deviceCodeManager = deviceCodeManager;
    }

    public async Task<DeviceFlowAuthorizedResult> DeviceAuthorizeAsync(string userCode, ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var valid = await _deviceCodeManager.IsUserCodeValidAsync(userCode, cancellationToken);

        if (!valid)
            return new DeviceFlowAuthorizedResult(false);

        await _deviceCodeManager.SetUserAuthorizedAsync(userCode, principal, cancellationToken);

        return new DeviceFlowAuthorizedResult(true);
    }
}
