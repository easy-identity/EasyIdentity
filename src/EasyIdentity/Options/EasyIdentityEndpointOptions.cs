namespace EasyIdentity.Options;

public class EasyIdentityEndpointOptions
{
    public string DiscoveryPath { get; set; } = "/.well-known/openid-configuration";
    public string JwksPath { get; set; } = "/.well-known/jwks";
    public string TokenPath { get; set; } = "/connect/token";
    public string AuthorizationPath { get; set; } = "/connect/authorize";
    public string UserinfoPath { get; set; } = "/connect/userinfo";
    public string EndSessionPath { get; set; } = "/connect/endsession";
    public string DeviceAuthorizationPath { get; set; } = "/connect/device";
    public string RegistrationPath { get; set; } = "/connect/registration";
    public string RevocationPath { get; set; } = "/connect/revocation";
    public string IntrospectionPath { get; set; } = "/connect/introspect";
    public string EndSession { get; set; } = "/connect/endsession";
    public string EndSessionCallback { get; set; } = "/connect/endsession/callback"; 
}
