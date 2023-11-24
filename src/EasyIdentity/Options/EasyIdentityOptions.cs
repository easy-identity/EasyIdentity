namespace EasyIdentity.Options;

public class EasyIdentityOptions
{
    public bool EncryptAccessToken { get; set; }

    public EasyIdentityAuthenticationOptions Authentication { get; } = new EasyIdentityAuthenticationOptions();
    public EasyIdentityTokenOptions Token { get; set; } = new EasyIdentityTokenOptions();
    public EasyIdentityEndpointOptions Endpoint { get; set; } = new EasyIdentityEndpointOptions();
    public EasyIdentityDeviceAuthorizationOptions DeviceAuthorization { get; set; } = new EasyIdentityDeviceAuthorizationOptions();
}

public class EasyIdentityCredentialOptions
{
}