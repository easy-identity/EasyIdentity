//using System.Net.Http.Headers;
//using System.Threading;
//using System.Threading.Tasks;
//using EasyIdentity.Models;
//using Microsoft.AspNetCore.Http;

//namespace EasyIdentity.Services;

//public class RequestAuthorizationParser : IRequestAuthorizationParser
//{
//    public Task<RequestAuthorization?> ParseAsync(HttpContext context, CancellationToken cancellationToken = default)
//    {
//        var authoriazationString = context.Request.Headers.Authorization;

//        RequestAuthorization? seret = null;

//        if (AuthenticationHeaderValue.TryParse(authoriazationString, out var auth))
//        {
//            seret = new RequestAuthorization(auth.Scheme, auth.Parameter!);
//        }

//        return Task.FromResult(seret);
//    }
//}
