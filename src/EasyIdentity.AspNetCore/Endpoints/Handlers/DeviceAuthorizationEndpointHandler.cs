using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/connect/device", Method = "POST")]
public class DeviceAuthorizationEndpointHandler : EndpointHandler
{
    public override string Name => "device_authorization";

    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IClientRequestValidator ClientValidator { get; }
    protected ITokenManager TokenManager { get; }
    protected IDeviceFlowManager DeviceCodeManager { get; }
    protected IDeviceAuthorizationUrlProvider DeviceAuthorizationUrlProvider { get; }

    public DeviceAuthorizationEndpointHandler(
        IOptions<EasyIdentityOptions> options,
        IClientRequestValidator clientValidator,
        ITokenManager tokenManager,
        IDeviceFlowManager deviceCodeManager,
        IDeviceAuthorizationUrlProvider deviceAuthorizationUrlProvider)
    {
        Options = options;
        ClientValidator = clientValidator;
        TokenManager = tokenManager;
        DeviceCodeManager = deviceCodeManager;
        DeviceAuthorizationUrlProvider = deviceAuthorizationUrlProvider;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var clientId = requestData.ClientId;

        var clientValidationResult = await ClientValidator.ValidateAsync(Name, clientId!, requestData, cancellationToken);

        if (!clientValidationResult.Succeeded)
        {
            return new ErrorResult(requestData, clientValidationResult.Failure);
        }

        var deviceCode = await DeviceCodeManager.GenerateDeviceCodeAsync(clientValidationResult.Client, requestData, cancellationToken);
        var userCode = await DeviceCodeManager.GenerateUserCodeAsync(clientValidationResult.Client, deviceCode, requestData, cancellationToken);

        var options = Options.Value;

        var verificationUri = await DeviceAuthorizationUrlProvider.GetVerificationUriAsync(clientValidationResult.Client, cancellationToken);

        return new DeviceAuthorizationResult(new DeviceAuthorizationResultData
        {
            DeviceCode = deviceCode,
            UserCode = userCode,
            ExpiresIn = (int)options.Token.DefaultDeviceCodeLifetime.TotalSeconds,
            Interval = options.DeviceAuthorization.DefaultDevicePollingInterval,
            VerificationUri = verificationUri.ToString(),
        });
    }
}
