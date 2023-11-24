using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class IssuerUriProvider : IIssuerUriProvider
{
    protected IHttpContextAccessor ContextAccessor { get; }

    public IssuerUriProvider(IHttpContextAccessor contextAccessor)
    {
        ContextAccessor = contextAccessor;
    }

    public Task<Uri> GetAsync()
    {
        var request = (ContextAccessor.HttpContext?.Request) ?? throw new InvalidOperationException("The http context must not be null");

        var issuer = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase}";

        return Task.FromResult(new Uri(issuer));
    }
}
