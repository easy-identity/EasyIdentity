using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Endpoints.Results;

public class AuthorizationResultExecutor : IEndpointResultExecutor<AuthorizationCodeResult>
{
    protected ILogger<AuthorizationResultExecutor> Logger { get; }

    public AuthorizationResultExecutor(ILogger<AuthorizationResultExecutor> logger)
    {
        Logger = logger;
    }

    public async Task ExecuteAsync(HttpContext context, AuthorizationCodeResult result, CancellationToken cancellationToken = default)
    {
        // Logger.LogDebug("Execute result by ");

        var redirectUrl = result.Request.RedirectUri;
        var state = result.Request.State;
        var responseMode = result.Request.ResponseMode;
        var code = result.Code;

        if (responseMode == "form_post")
        {
            var html = ResponseHelper.BuilderFormPostHtml(redirectUrl, new Dictionary<string, string?> { { "code", code }, { "state", state } });

            context.Response.SetNoCache();
            await context.Response.WriteHtmlAsync(html);
        }
        else if (responseMode == "fragment")
        {
            context.Response.Redirect($"{redirectUrl}#code={code}&state={state}");
        }
        else
        {
            context.Response.Redirect($"{redirectUrl}?code={code}&state={state}");
        }
    }
}