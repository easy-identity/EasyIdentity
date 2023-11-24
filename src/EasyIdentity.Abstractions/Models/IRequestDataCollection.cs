using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace EasyIdentity.Models;

/// <summary>
///  represent an values by http request
/// </summary>
public interface IRequestCollection : IEnumerable<KeyValuePair<string, StringValues>>
{
    int Count { get; }
    ICollection<string> Keys { get; }
    bool ContainsKey(string key);
    bool TryGetValue(string key, out StringValues value);
    StringValues this[string key] { get; }

    RequestAuthorization Authorization { get; }

    string RequestPath { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Headers { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Query { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Form { get; }

    #region quick access    

    StringValues GrantType { get; }
    StringValues ClientId { get; }
    StringValues ClientSecret { get; }
    StringValues Scope { get; }
    string[] Scopes { get; }
    StringValues Code { get; }
    StringValues RedirectUri { get; }
    StringValues State { get; }
    StringValues Nonce { get; }
    StringValues Username { get; }
    StringValues Password { get; }
    StringValues RefreshToken { get; }
    StringValues DeviceCode { get; }
    StringValues Tenant { get; }
    /// <summary>
    ///  like none/login/consent/select_account
    /// </summary>
    StringValues Prompt { get; }
    StringValues LoginHint { get; }
    StringValues DomainHint { get; }
    StringValues CodeChallenge { get; }
    StringValues CodeChallengeMethod { get; }
    StringValues CodeVerifier { get; }
    StringValues ClientAssertionType { get; }
    StringValues ClientAssertion { get; }
    StringValues TenantId { get; }

    StringValues ResponseType { get; }
    string[] ResponseTypes { get; }

    /// <summary>
    ///  'response_mode'
    /// </summary>
    /// <remarks>
    ///  fragment/form_post/query
    /// </remarks>
    StringValues ResponseMode { get; }

    #endregion quick access    
}