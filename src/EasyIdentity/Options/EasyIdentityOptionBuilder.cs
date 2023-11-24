using System;
using EasyIdentity.Services;
using EasyIdentity.Services.TokenProviders;
using EasyIdentity.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyIdentity.Options;

public class EasyIdentityOptionBuilder
{
    public IServiceCollection Services { get; }
    public EasyIdentityOptions Options { get; }

    public EasyIdentityOptionBuilder(IServiceCollection services, EasyIdentityOptions options)
    {
        Services = services;
        Options = options;
    }

    private void AddTokenHandlers()
    {
        AddTokenHandler<AuthorizationCodeTokenProvider>();
        AddTokenHandler<ImplicitTokenProvider>();
        AddTokenHandler<ClientCredentialsTokenProvider>();
        AddTokenHandler<DeviceTokenProvider>();
        AddTokenHandler<PasswordGrantTokenProvider>();
        AddTokenHandler<RefreshTokenProvider>();
    }

    private void AddStores()
    {
        AddAuthorizationCodeStore<AuthorizationCodeStore>();
        AddDeviceCodeStore<DeviceFlowStore>();
        AddTokenStore<TokenStore>();
    }

    private void AddServices()
    {
        AddUserIdentityProvider<NullUserProvider>();

        Services.TryAddScoped<IGrantTypeTokenProviderFactory, GrantTypeTokenProviderFactory>();

        Services.TryAddScoped<IJsonSerializer, SystemTextJsonSerializer>();

        Services.TryAddScoped<ITokenGenerator, JwtTokenGenerator>();
        Services.TryAddScoped<ITokenManager, TokenManager>();

        Services.TryAddScoped<IAuthorizationCodeManager, AuthorizationCodeManager>();
        Services.TryAddScoped<IAuthorizationCodeGenerator, AuthorizationCodeGenerator>();

        Services.TryAddScoped<IClientClaimsPrincipalFactory, ClientClaimsPrincipalFactory>();

        Services.TryAddScoped<IAuthorizationService, AlwaysGrantedAuthorizationService>();

        Services.TryAddScoped<IClientManager, ClientManager>();
        Services.TryAddScoped<IClientSecretValidator, ClientSecretValidator>();
        Services.TryAddScoped<IClientRequestValidator, ClientRequestValidator>();
        Services.TryAddScoped<IClientRedirectUrlValidator, ClientRedirectUrlValidator>();

        Services.TryAddScoped<IDeviceCodeGenerator, DeviceCodeGenerator>();
        Services.TryAddScoped<IDeviceFlowManager, DeviceFlowManager>();
    }

    public EasyIdentityOptionBuilder AddCoreServices()
    {
        AddTokenHandlers();
        AddStores();
        AddServices();

        return this;
    }

    public EasyIdentityOptionBuilder AddUserIdentityProvider<T>() where T : class, IUserProvider
    {
        Services.Replace(ServiceDescriptor.Transient<IUserProvider, T>());
        return this;
    }

    public EasyIdentityOptionBuilder AddTokenHandler<T>() where T : class, IGrantTypeTokenProvider
    {
        Services.AddTransient<IGrantTypeTokenProvider, T>();

        return this;
    }

    public EasyIdentityOptionBuilder AddAuthorizationCodeStore<T>() where T : class, IAuthorizationCodeStore
    {
        Services.Replace(ServiceDescriptor.Transient<IAuthorizationCodeStore, T>());
        return this;
    }

    public EasyIdentityOptionBuilder AddClientStore<T>() where T : class, IClientStore
    {
        Services.Replace(ServiceDescriptor.Transient<IClientStore, T>());
        return this;
    }

    public EasyIdentityOptionBuilder AddDeviceCodeStore<T>() where T : class, IDeviceFlowStore
    {
        Services.Replace(ServiceDescriptor.Transient<IDeviceFlowStore, T>());
        return this;
    }

    public EasyIdentityOptionBuilder AddScopeStore<T>() where T : class, IScopeStore
    {
        Services.Replace(ServiceDescriptor.Transient<IScopeStore, T>());
        return this;
    }

    public EasyIdentityOptionBuilder AddTokenStore<T>() where T : class, ITokenStore
    {
        Services.Replace(ServiceDescriptor.Transient<ITokenStore, T>());
        return this;
    }

    public void AddStaticStore(Action<StaticStoreBuilder> configure)
    {
        configure.Invoke(new StaticStoreBuilder(this));
    }

    public void LoadFromConfigrations(IConfigurationSection configurationSection)
    {
        // TODO
    }
}
