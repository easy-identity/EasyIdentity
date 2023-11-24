using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IDeviceAuthorizationUrlProvider
{
    Task<Uri> GetVerificationUriAsync(Client client, CancellationToken cancellationToken = default);
}
