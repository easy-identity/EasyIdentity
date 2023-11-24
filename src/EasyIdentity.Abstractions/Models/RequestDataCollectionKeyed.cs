using Microsoft.Extensions.Primitives;

namespace EasyIdentity.Models;

public partial class RequestDataCollection
{
    public StringValues GrantType => this["grant_type"];
    public StringValues ClientId => GetClientIdFromBasicAuth() ?? this["client_id"];
    public StringValues ClientSecret => GetClientSecretFromBasicAuth() ?? this["client_secret"];
    public StringValues Scope => this["scope"];
    public string[] Scopes => Scope.Count == 0 ? new string[0] : Scope.ToString().Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
    public StringValues Code => this["code"];
    public StringValues RedirectUri => this["redirect_uri"];
    public StringValues State => this["state"];
    public StringValues Nonce => this["nonce"];
    public StringValues Username => this["username"];
    public StringValues Password => this["password"];
    public StringValues RefreshToken => this["refresh_token"];
    public StringValues DeviceCode => this["device_code"];
    public StringValues Tenant => this["tenant"];
    /// <summary>
    ///  like none/login/consent/select_account
    /// </summary>
    public StringValues Prompt => this["prompt"];
    public StringValues LoginHint => this["login_hint"];
    public StringValues DomainHint => this["domain_hint"];
    public StringValues CodeChallenge => this["code_challenge"];
    public StringValues CodeChallengeMethod => this["code_challenge_method"];
    public StringValues CodeVerifier => this["code_verifier"];
    public StringValues ClientAssertionType => this["client_assertion_type"];
    public StringValues ClientAssertion => this["client_assertion"];
    public StringValues TenantId => this["tenant_id"];

    public StringValues ResponseType => this["response_type"];
    public string[] ResponseTypes => ResponseType.ToString().Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    ///  'response_mode'
    /// </summary>
    /// <remarks>
    ///  fragment/form_post/query
    /// </remarks>
    public StringValues ResponseMode => this["response_mode"];

    private string? GetClientSecretFromBasicAuth()
    {
        if (Authorization?.IsBasicAuth() == true)
        {
            var basicAuth = Authorization.GetBasicAuth();

            return basicAuth?.Length == 2 ? basicAuth[1] : null;
        }

        return null;
    }

    private string? GetClientIdFromBasicAuth()
    {
        if (Authorization?.IsBasicAuth() == true)
        {
            var basicAuth = Authorization.GetBasicAuth();

            return basicAuth?.Length > 0 ? basicAuth[0] : null;
        }

        return null;
    }
}
