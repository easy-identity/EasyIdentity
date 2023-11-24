using System.Collections.Generic;

namespace EasyIdentity.Constants;

public static class ResponseTypes
{
    public static readonly List<string> All =
    [
        "code",
        "token",
        "id_token",
        "id_token token",
        "code id_token",
        "code token",
        "code id_token token",
    ];
}
