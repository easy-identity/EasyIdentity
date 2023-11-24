using System;
using EasyIdentity.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddEasyIdentityCookies(this AuthenticationBuilder builder, Action<CookieAuthenticationOptions> configure)
    {
        return builder.AddCookie(EasyIdentityConsts.ApplicationScheme, configure);
    }
}