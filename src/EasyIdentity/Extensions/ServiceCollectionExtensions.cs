using System;
using EasyIdentity.Options;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///  Add easy identity core services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    public static EasyIdentityOptionBuilder AddEasyIdentityCore(this IServiceCollection services, Action<EasyIdentityOptions> configure)
    {
        EasyIdentityOptions options = new EasyIdentityOptions();

        configure?.Invoke(options);

        services.AddOptions<EasyIdentityOptions>();

        var optionBuilder = new EasyIdentityOptionBuilder(services, options);

        optionBuilder.AddCoreServices();

        services.AddSingleton(optionBuilder);

        return optionBuilder;
    }
}
