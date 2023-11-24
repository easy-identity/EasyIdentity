namespace EasyIdentity.Constants;

/**
     https://www.oauth.com/oauth2-servers/server-side-apps/possible-errors/
     */
public static class CommonErrorCode
{
    public const string INVALID_REQUEST = "invalid_request";
    public const string INVALID_CLIENT = "invalid_client";
    public const string INVALID_GRANT = "invalid_grant";
    public const string UNAUTHORIZED_CLIENT = "unauthorized_client";
    public const string UNSUPPORTED_GRANT_TYPE = "unsupported_grant_type";
    public const string UNSUPPORTED_RESPONSE_TYPE = "unsupported_response_type";
    public const string INVALID_SCOPE = "invalid_scope";
    public const string INVALID_RESOURCE = "invalid_resource";
    public const string SERVER_ERROR = "server_error";
    public const string TEMPORARILY_UNAVAILABLE = "temporarily_unavailable";
}
