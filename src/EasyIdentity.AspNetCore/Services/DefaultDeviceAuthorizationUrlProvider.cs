using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class DefaultDeviceAuthorizationUrlProvider : IDeviceAuthorizationUrlProvider
{
    private IHttpContextAccessor HttpContextAccessor { get; }

    public DefaultDeviceAuthorizationUrlProvider(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public Task<Uri> GetVerificationUriAsync(Client client, CancellationToken cancellationToken = default)
    {
        var request = (HttpContextAccessor.HttpContext?.Request) ?? throw new InvalidOperationException("The http context must not be null");

        var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase}";

        return Task.FromResult(new Uri(baseUrl + "/device"));
    }
}