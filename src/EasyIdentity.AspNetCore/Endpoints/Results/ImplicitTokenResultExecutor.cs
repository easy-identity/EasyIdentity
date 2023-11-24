using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints.Results;

public class ImplicitTokenResultExecutor : IEndpointResultExecutor<ImplicitTokenResult>
{
    public async Task ExecuteAsync(HttpContext context, ImplicitTokenResult result, CancellationToken cancellationToken = default)
    {
        var redirectUrl = result.Request.RedirectUri;
        var state = result.Request.State;
        var nonce = result.Request.Nonce;
        var responseMode = result.Request.ResponseMode;
        var token = result.Token;

        var tokenData = new Dictionary<string, string?>
        {
            ["state"] = state,
            ["access_token"] = token.AccessToken,
            ["id_token"] = token.IdentityToken,
            ["expires_in"] = token.AccessTokenLifetime.TotalSeconds.ToString(),
            ["scope"] = string.Join(" ", result.Request.Scopes)
        };

        if (result.Request.ResponseTypes.Contains("token"))
            tokenData["token_type"] = "Bearer";
        if (result.Request.ResponseTypes.Contains("code"))
            tokenData["code"] = result.Code;

        if (!string.IsNullOrWhiteSpace(nonce))
            tokenData["nonce"] = nonce;

        if (responseMode == "form_post")
        {
            var html = ResponseHelper.BuilderFormPostHtml(redirectUrl, tokenData);

            context.Response.SetNoCache();
            await context.Response.WriteHtmlAsync(html);
        }
        else if (responseMode == "fragment")
        {
            context.Response.Redirect($"{redirectUrl}#{string.Join("&", tokenData.Select(x => x.Key + "=" + x.Value))}");
        }
        else
        {
            context.Response.Redirect($"{redirectUrl}?{string.Join("&", tokenData.Select(x => x.Key + "=" + x.Value))}");
        }
    }
}