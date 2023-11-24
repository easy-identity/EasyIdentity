using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class GrantTypeTokenProviderFactory : IGrantTypeTokenProviderFactory
{
    private readonly ILogger<GrantTypeTokenProviderFactory> _logger;
    private readonly IEnumerable<IGrantTypeTokenProvider> _providers;

    public GrantTypeTokenProviderFactory(ILogger<GrantTypeTokenProviderFactory> logger, IEnumerable<IGrantTypeTokenProvider> providers)
    {
        _logger = logger;
        _providers = providers;
    }

    public IGrantTypeTokenProvider Get(string grantType)
    {
        var provider = _providers.FirstOrDefault(x => x.Name == grantType);

        if (provider == null)
        {
            _logger.LogError("The grant type '{0}' token provider not found.", grantType);

            throw new System.Exception(string.Format("The grant type '{0}' token provider not found.", grantType));
        }

        return provider;
    }
}
