using System;
using EasyIdentity.Constants;
using EasyIdentity.Endpoints.Handlers;
using EasyIdentity.Endpoints.Results;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RedirectResultExecutor = EasyIdentity.Endpoints.Results.RedirectResultExecutor;

namespace EasyIdentity.Extensions;

public static class ServiceCollectionExtensions
{
    public static EasyIdentityOptionBuilder AddEasyIdentity(this IServiceCollection services)
    {
        return AddEasyIdentity(services, _ => { });
    }

    public static EasyIdentityOptionBuilder AddEasyIdentity(this IServiceCollection services, Action<EasyIdentityOptions> configure)
    {
        // dependencies
        services.AddHttpContextAccessor();

        var builder = services.AddEasyIdentityCore(configure);

        AddEndpointHandler(builder);
        AddEndpointResultExecutor(builder);

        builder.Services.TryAddScoped<IRequestReader, DefaultRequestReader>();
        builder.Services.TryAddScoped<IIssuerUriProvider, IssuerUriProvider>();
        builder.Services.TryAddScoped<IDeviceAuthorizationUrlProvider, DefaultDeviceAuthorizationUrlProvider>();

        builder.Services.TryAddScoped<IInteractionService, InteractionService>();

        builder.Services.Replace(ServiceDescriptor.Scoped<IAuthorizationService, AuthorizationService>());

        return builder;
    }

    public static IServiceCollection ConfigureEasyIdentityCookies(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
    {
        return services.Configure(EasyIdentityConsts.ApplicationScheme, configure);
    }

    public static void AddAddEasyIdentityCookies() { }

    private static void AddEndpointHandler(EasyIdentityOptionBuilder builder)
    {
        builder.AddEndpointHandler<AuthorizationEndpointHandler>();
        builder.AddEndpointHandler<DeviceAuthorizationEndpointHandler>();
        builder.AddEndpointHandler<DiscoveryEndpointHandler>();
        builder.AddEndpointHandler<JwksEndpointHandler>();
        builder.AddEndpointHandler<TokenEndpointHandler>();
        builder.AddEndpointHandler<UserinfoEndpointHanlder>();
    }

    private static void AddEndpointResultExecutor(EasyIdentityOptionBuilder builder)
    {
        builder.AddEndpointResultExecutor<AuthorizationCodeResult, AuthorizationResultExecutor>();
        builder.AddEndpointResultExecutor<ImplicitTokenResult, ImplicitTokenResultExecutor>();
        builder.AddEndpointResultExecutor<ChallengeResult, ChallengeResultExecutor>();
        builder.AddEndpointResultExecutor<ConsentResult, ConsentResultExecutor>();
        builder.AddEndpointResultExecutor<DeviceAuthorizationResult, DeviceAuthorizationResultExecutor>();
        builder.AddEndpointResultExecutor<DiscoveryInfoResult, DiscoveryInfoResultExecutor>();
        builder.AddEndpointResultExecutor<ErrorResult, ErrorResultExecutor>();
        builder.AddEndpointResultExecutor<JwksResult, JwksResultExecutor>();
        builder.AddEndpointResultExecutor<NoopResult, NoopResultExecutor>();
        builder.AddEndpointResultExecutor<RedirectResult, RedirectResultExecutor>();
        builder.AddEndpointResultExecutor<TokenResult, TokenResultExecutor>();
        builder.AddEndpointResultExecutor<UnauthenticatedResult, UnauthenticatedResultExecutor>();
        builder.AddEndpointResultExecutor<UserInfoResult, UserInfoResultExecutor>();
    }
}
