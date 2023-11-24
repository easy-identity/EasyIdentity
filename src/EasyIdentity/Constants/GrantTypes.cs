using System.Collections.Generic;

namespace EasyIdentity.Constants;

public static class GrantTypes
{
    public const string ClientCredentials = "client_credentials";
    public const string AuthorizationCode = "authorization_code";
    public const string Password = "password";
    public const string Implicit = "implicit";
    public const string RefreshToken = "refresh_token";
    public const string DeviceCode = "urn:ietf:params:oauth:grant-type:device_code";
    //public const string Ciba = "urn:openid:params:grant-type:ciba";

    public static List<string> All => [ClientCredentials, AuthorizationCode, Password, Implicit, RefreshToken, DeviceCode];
}
