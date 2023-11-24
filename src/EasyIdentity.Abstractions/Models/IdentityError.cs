using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EasyIdentity.Models;

/// <summary>
///  The common error model
/// </summary>
public class Error
{
    /**
     https://www.oauth.com/oauth2-servers/server-side-apps/possible-errors/
     */
    public const string INVALID_REQUEST = "invalid_request";
    public const string INVALID_CLIENT = "invalid_client";
    public const string INVALID_GRANT = "invalid_grant";
    public const string UNAUTHORIZED_CLIENT = "unauthorized_client";
    public const string UNSUPPORTED_GRANT_TYPE = "unsupported_grant_type";
    public const string UNSUPPORTED_RESPONSE_TYPE = "unsupported_response_type";
    public const string INVALID_SCOPE = "invalid_scope";
    public const string SERVER_ERROR = "server_error";
    public const string TEMPORARILY_UNAVAILABLE = "temporarily_unavailable";

    [JsonPropertyName("error")]
    public string Code { get; } = null!;

    [JsonPropertyName("error_description")]
    public string? Description { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object>? Extension { get; set; }

    protected Error()
    {
    }

    public override string ToString()
    {
        return Code;
    }

    public Error(string code, string? description = null, Dictionary<string, object>? extensionData = null)
    {
        Code = code;
        Description = description;
        Extension = extensionData;
    }

    public static Error Create(string code, string? description = null, Dictionary<string, object>? extensions = null)
    {
        return new Error(code, description, extensions);
    }
}
