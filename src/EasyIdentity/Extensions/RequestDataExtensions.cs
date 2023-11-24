using System;
using System.Text;
using System.Web;
using EasyIdentity.Models;

namespace EasyIdentity.Extensions;

public static class RequestDataExtensions
{
    //public static string? GetBearerToken(this IRequestDataCollection requestData)
    //{
    //    var authorization = requestData.Authorization;
    //    return authorization.StartsWith("Bearer", System.StringComparison.InvariantCultureIgnoreCase)
    //        ? (authorization.Substring(7)?.Trim())
    //        : null;
    //}

    //public static (string name, string password)? GetBasicAuthorization(this IRequestDataCollection requestData)
    //{
    //    var authorization = requestData.Authorization;
    //    if (authorization.StartsWith("Basic", System.StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        var base64String = authorization.Substring(6)?.Trim();
    //        string tmp = string.Empty;
    //        try
    //        {
    //            tmp = Encoding.UTF8.GetString(Convert.FromBase64String(base64String!));
    //        }
    //        catch
    //        {
    //        }

    //        var password = string.Empty;
    //        var userName = tmp.Split('@')[0];
    //        if (tmp.Split('@').Length > 1)
    //            password = tmp.Split('@')[1];

    //        return (HttpUtility.UrlDecode(userName), HttpUtility.UrlDecode(password));
    //    }

    //    return null;
    //}
}
