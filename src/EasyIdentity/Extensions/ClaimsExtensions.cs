using System.Security.Claims;

namespace EasyIdentity.Extensions;

public static class ClaimsExtensions
{
    public static string GetSubject(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
    }

    public static string GetSubject(this ClaimsIdentity claimsIdentity)
    {
        return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
