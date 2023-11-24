using System.Collections.Generic;

namespace EasyIdentity.Constants;

public static class Consts
{
    public static class PromptMode
    {
        public const string None = "none";
        public const string Login = "login";
        public const string Consent = "consent";
        public const string SelectAccount = "select_account";
    }

    public static readonly List<string> PromptModes =
    [
        PromptMode.None,
        PromptMode.Login,
        PromptMode.Consent,
        PromptMode.SelectAccount
    ];

    public static class ResponseModesSupported
    {
        public const string FormPost = "form_post";
        public const string Query = "query";
        public const string Fragment = "fragment";
    }

    public static readonly List<string> ResponseModesSupporteds = [
        ResponseModesSupported.FormPost,
        ResponseModesSupported.Query,
        ResponseModesSupported.Fragment,
    ];

    public static class CodeChallengeMethods
    {
        public const string Plain = "plain";
        public const string Sha256 = "S256";
    }

    public static readonly List<string> SupportedCodeChallengeMethods =
    [
        CodeChallengeMethods.Plain,
        CodeChallengeMethods.Sha256
    ];

    public static class KnownAcrValueName
    {
        public const string Idp = "idp:";
        public const string Tenant = "tenant:";
    }

    public static readonly List<string> KnownAcrValues =
    [
        KnownAcrValueName.Idp,
        KnownAcrValueName.Tenant
    ];

    public static List<string> AllStandardScopes = [
        StandardScopes.OpenId,
        StandardScopes.Profile,
        StandardScopes.Email,
        StandardScopes.Address,
        StandardScopes.Phone,
        StandardScopes.OfflineAccess,
    ];
}
