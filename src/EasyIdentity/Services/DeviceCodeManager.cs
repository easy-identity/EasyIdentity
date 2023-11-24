using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentity.Options;
using EasyIdentity.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class DeviceFlowManager : IDeviceFlowManager
{
    protected ILogger<DeviceFlowManager> Logger { get; }
    protected IOptions<EasyIdentityOptions> Options { get; }
    protected IDeviceFlowStore DeviceCodeStore { get; }
    protected IDeviceCodeGenerator DeviceCodeGenerator { get; }

    public DeviceFlowManager(ILogger<DeviceFlowManager> logger, IDeviceFlowStore deviceCodeStore, IDeviceCodeGenerator deviceCodeGenerator, IOptions<EasyIdentityOptions> options)
    {
        Logger = logger;
        DeviceCodeStore = deviceCodeStore;
        DeviceCodeGenerator = deviceCodeGenerator;
        Options = options;
    }

    public virtual async Task<string> GenerateDeviceCodeAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        return await DeviceCodeGenerator.GenerateDeviceCodeAsync(client, requestData.Scopes, cancellationToken);
    }

    public virtual async Task<string> GenerateUserCodeAsync(Client client, string deviceCode, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var userCode = await DeviceCodeGenerator.GenerateUserCodeAsync(client, requestData.Scopes, cancellationToken);

        await DeviceCodeStore.SaveAsync(client, deviceCode, userCode, requestData.Scopes, client.Token.DeviceCodeLifetime ?? Options.Value.Token.DefaultDeviceCodeLifetime);

        return userCode;
    }

    public virtual async Task<DeviceCodeValidationResult> ValidateDeviceCodeAsync(Client client, string deviceCode, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(deviceCode))
        {
            throw new ArgumentException($"'{nameof(deviceCode)}' cannot be null or whitespace.", nameof(deviceCode));
        }

        var model = await DeviceCodeStore.FindByDeviceCodeAsync(client, deviceCode, cancellationToken);

        if (model == null)
        {
            return new DeviceCodeValidationResult(DeviceCodeAuthorizedState.Expired);
        }

        if (model.CreationTime.Add(model.Lifetime) < DateTime.UtcNow)
        {
            return new DeviceCodeValidationResult(DeviceCodeAuthorizedState.Expired);
        }

        if (model.IsReject)
        {
            return new DeviceCodeValidationResult(DeviceCodeAuthorizedState.Declined, model.Subject!);
        }

        return model.IsAuthorized
            ? new DeviceCodeValidationResult(DeviceCodeAuthorizedState.Granted, model.Subject!)
            : new DeviceCodeValidationResult(DeviceCodeAuthorizedState.Pending);
    }

    public async Task<bool> IsUserCodeValidAsync(string userCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            throw new ArgumentException($"'{nameof(userCode)}' cannot be null or whitespace.", nameof(userCode));
        }

        var model = await DeviceCodeStore.FindByUserCodeAsync(userCode, cancellationToken);

        if (model == null)
        {
            return false;
        }

        if (model.CreationTime.Add(model.Lifetime) < DateTime.UtcNow)
        {
            return false;
        }

        return !model.IsReject && !model.IsAuthorized;
    }

    public async Task SetUserAuthorizedAsync(string userCode, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            throw new ArgumentException($"'{nameof(userCode)}' cannot be null or whitespace.", nameof(userCode));
        }

        var model = await DeviceCodeStore.FindByUserCodeAsync(userCode, cancellationToken);

        if (model == null)
        {
            throw new InvalidOperationException("The user code is not valid");
        }

        if (model.CreationTime.Add(model.Lifetime) < DateTime.UtcNow)
        {
            throw new InvalidOperationException("The user code is expired");
        }

        if (model.IsReject || model.IsAuthorized)
        {
            throw new InvalidOperationException("The user code is reject or authorized");
        }

        await DeviceCodeStore.UpdateSubjectByDeviceCodeAsync(model.DeviceCode, claimsPrincipal.GetSubject(), true);
    }

    public async Task SetUserRejectdAsync(string userCode, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userCode))
        {
            throw new ArgumentException($"'{nameof(userCode)}' cannot be null or whitespace.", nameof(userCode));
        }

        var model = await DeviceCodeStore.FindByUserCodeAsync(userCode, cancellationToken);

        if (model == null)
        {
            throw new InvalidOperationException("The user code is not valid");
        }

        if (model.CreationTime.Add(model.Lifetime) < DateTime.UtcNow)
        {
            throw new InvalidOperationException("The user code is expired");
        }

        if (model.IsReject || model.IsAuthorized)
        {
            throw new InvalidOperationException("The user code is reject or authorized");
        }

        await DeviceCodeStore.UpdateSubjectByDeviceCodeAsync(model.DeviceCode, claimsPrincipal.GetSubject(), false);
    }

    public async Task<string[]> GetScopesByDeviceCodeAsync(Client client, string deviceCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(deviceCode))
        {
            throw new ArgumentException($"'{nameof(deviceCode)}' cannot be null or whitespace.", nameof(deviceCode));
        }

        var model = await DeviceCodeStore.FindByDeviceCodeAsync(client, deviceCode, cancellationToken);

        if (model == null)
        {
            throw new InvalidOperationException("The device code is invalid");
        }

        return model.Scopes;
    }
}
