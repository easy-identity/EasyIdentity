using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class EasyIdentityOptionExtensions
{
    /// <summary>
    ///  Adapter Microsoft.Extensions.Identity
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="builder"></param>
    public static EasyIdentityOptionBuilder AddIdentityExtension<TUser>(this EasyIdentityOptionBuilder builder) where TUser : class
    {
        builder.AddUserIdentityProvider<UserProvider<TUser>>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });

        builder.Services.Configure<EasyIdentityOptions>(options => options.Authentication.AuthenticationScheme = IdentityConstants.ApplicationScheme);

        return builder;
    }
}
