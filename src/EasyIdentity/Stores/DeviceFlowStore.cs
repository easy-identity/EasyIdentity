using EasyIdentity.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public class DeviceFlowStore : IDeviceFlowStore
{
    private static readonly List<DeviceFlowData> _store = [];

    public Task<DeviceFlowData?> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default)
    {
        DeviceFlowData? result = null;
        lock (_store)
        {
            result = _store.Find(x => x.UserCode == userCode);
        }

        return Task.FromResult<DeviceFlowData?>(result);
    }

    public virtual Task<DeviceFlowData?> FindByDeviceCodeAsync(Client client, string deviceCode, CancellationToken cancellationToken = default)
    {
        DeviceFlowData? result = null;
        lock (_store)
        {
            result = _store.Find(x => x.DeviceCode == deviceCode);
        }

        return Task.FromResult<DeviceFlowData?>(result);
    }

    public virtual Task<DeviceFlowData> SaveAsync(Client client, string deviceCode, string userCode, string[] scopes, TimeSpan lifttime, CancellationToken cancellationToken = default)
    {
        var data = new DeviceFlowData()
        {
            CreationTime = DateTime.UtcNow,
            Scopes = scopes,
            Lifetime = lifttime,
            ClientId = client.Name,
            DeviceCode = deviceCode,
            UserCode = userCode,
        };
        lock (_store)
        {
            _store.Add(data);
        }
        return Task.FromResult(data);
    }

    public Task UpdateSubjectByDeviceCodeAsync(string deviceCode, string subject, bool isAuthorize, CancellationToken cancellationToken = default)
    {
        lock (_store)
        {
            var data = _store.Find(x => x.DeviceCode == deviceCode);

            if (data == null)
            {
                throw new InvalidOperationException("The data not found.");
            }

            data.Subject = subject;
            data.IsAuthorized = isAuthorize;
            data.IsReject = !isAuthorize;
        }

        return Task.CompletedTask;
    }

    public Task DeleteByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default)
    {
        lock (_store)
        {
            var result = _store.Find(x => x.DeviceCode == deviceCode);

            if (result != null)
            {
                _store.Remove(result);
            }
        }

        return Task.CompletedTask;
    }
}
