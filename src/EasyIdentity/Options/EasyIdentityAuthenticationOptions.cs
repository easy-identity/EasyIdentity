using EasyIdentity.Constants;

namespace EasyIdentity.Options;

public class EasyIdentityAuthenticationOptions
{
    public string AuthenticationScheme { get; set; } = EasyIdentityConsts.ApplicationScheme;
    public string ConsentUrl { get; set; } = "/consent";
}
