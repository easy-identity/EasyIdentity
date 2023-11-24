using EasyIdentity.Models;
using EasyIdentity.Options;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Stores;

public class StaticStoreBuilder
{
    public StaticStoreBuilder(EasyIdentityOptionBuilder optionBuilder)
    {
        OptionBuilder = optionBuilder;
    }

    public EasyIdentityOptionBuilder OptionBuilder { get; }

    public StaticStoreBuilder Clients(params Client[] clients)
    {
        var instance = new StaticClientStore(clients);
        OptionBuilder.Services.AddSingleton<IClientStore>(instance);
        return this;
    }

    public StaticStoreBuilder CustomScopes(params string[] scopes)
    {
        var instance = new StaticScopeStore(scopes);
        OptionBuilder.Services.AddSingleton<IScopeStore>(instance);
        return this;
    }
}
