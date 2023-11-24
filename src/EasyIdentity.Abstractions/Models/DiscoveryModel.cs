namespace EasyIdentity.Models;

public class DiscoveryInfoModel
{
    public string? Issuer { get; set; }
    public string? TokenEndpoint { get; set; }
    public string? AuthorizationEndpoint { get; set; }
    public string? IntrospectionEndpoint { get; set; }
    public string? UserinfoEndpoint { get; set; }
    public string? EndSessionEndpoint { get; set; }
    public string? DeviceAuthorizationEndpoint { get; set; }
    public string? JwksUri { get; set; }
    public string? CheckSessionIframe { get; set; }
    public string[]? GrantTypesSupported { get; set; }
    public string[]? ResponseTypesSupported { get; set; }
    public string[]? SubjectTypesSupported { get; set; }
    public string[]? IdTokenSigningAlgValuesSupported { get; set; }
    public string[]? IdTokenEncryptionAlgValuesSupported { get; set; }
    public string[]? IdTokenEncryptionEncValuesSupported { get; set; }
    public string[]? UserinfoSigningAlgValuesSupported { get; set; }
    public string[]? UserinfoEncryptionAlgValuesSupported { get; set; }
    public string[]? UserinfoEncryptionEncValuesSupported { get; set; }
    public string[]? RequestObjectSigningAlgValuesSupported { get; set; }
    public string[]? RequestObjectEncryptionAlgValuesSupported { get; set; }
    public string[]? RequestObjectEncryptionEncValuesSupported { get; set; }
    public string[]? ResponseModesSupported { get; set; }
    public string? RegistrationEndpoint { get; set; }
    public string[]? TokenEndpointAuthMethodsSupported { get; set; }
    public string[]? TokenEndpointAuthSigningAlgValuesSupported { get; set; }
    public string[]? IntrospectionEndpointAuthMethodsSupported { get; set; }
    public string[]? IntrospectionEndpointAuthSigningAlgValuesSupported { get; set; }
    public string[]? AuthorizationSigningAlgValuesSupported { get; set; }
    public string[]? AuthorizationEncryptionAlgValuesSupported { get; set; }
    public string[]? AuthorizationEncryptionEncValuesSupported { get; set; }
    public string[]? ClaimsSupported { get; set; }
    public string[]? ClaimTypesSupported { get; set; }
    public bool ClaimsParameterSupported { get; set; }
    public string[]? ScopesSupported { get; set; }
    public bool RequestParameterSupported { get; set; }
    public bool RequestUriParameterSupported { get; set; }
    public bool RequireRequestUriRegistration { get; set; }
    public string[]? CodeChallengeMethodsSupported { get; set; }
    public bool TlsClientCertificateBoundAccessTokens { get; set; }
    public string? RevocationEndpoint { get; set; }
    public string[]? RevocationEndpointAuthMethodsSupported { get; set; }
    public string[]? RevocationEndpointAuthSigningAlgValuesSupported { get; set; }
    public bool BackchannelLogoutSupported { get; set; }
    public bool BackchannelLogoutSessionSupported { get; set; }
    public string[]? BackchannelTokenDeliveryModesSupported { get; set; }
    public string? BackchannelAuthenticationEndpoint { get; set; }
    public string[]? BackchannelAuthenticationRequestSigningAlgValuesSupported { get; set; }
    public bool RequirePushedAuthorizationRequests { get; set; }
    public string? PushedAuthorizationRequestEndpoint { get; set; }
}