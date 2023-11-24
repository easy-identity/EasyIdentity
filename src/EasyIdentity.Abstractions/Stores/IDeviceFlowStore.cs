using EasyIdentity.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Stores;

public interface IDeviceFlowStore
{
    Task<DeviceFlowData> SaveAsync(Client client, string deviceCode, string userCode, string[] scopes, TimeSpan lifttime, CancellationToken cancellationToken = default);

    Task<DeviceFlowData?> FindByDeviceCodeAsync(Client client, string deviceCode, CancellationToken cancellationToken = default);

    Task<DeviceFlowData?> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default);

    Task UpdateSubjectByDeviceCodeAsync(string deviceCode, string subject, bool isAuthorize, CancellationToken cancellationToken = default);

    Task DeleteByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default);
}
