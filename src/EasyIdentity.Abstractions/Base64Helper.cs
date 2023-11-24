using System;

namespace EasyIdentity;
public static class Base64Helper
{
    public static string Encode(byte[] source)
    {
        var result = Convert.ToBase64String(source);
        return result.Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}
