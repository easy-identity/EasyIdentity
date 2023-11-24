using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace EasyIdentity.Services;

public class DefaultRequestReader : IRequestReader
{
    protected ILogger<DefaultRequestReader> Logger { get; }

    public DefaultRequestReader(ILogger<DefaultRequestReader> logger)
    {
        Logger = logger;
    }

    public async ValueTask<IRequestCollection> ReadAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        var request = httpContext.Request;

        var headers = request.Headers.ToDictionary(x => x.Key, x => x.Value);
        var query = request.Query.ToDictionary(x => x.Key, x => x.Value);
        var requestPath = request.Path;

        RequestAuthorization? requestAuthorization = null;
        var authoriazationString = request.Headers.Authorization;
        if (AuthenticationHeaderValue.TryParse(authoriazationString, out var auth))
        {
            requestAuthorization = new RequestAuthorization(auth.Scheme, auth.Parameter!);
        }

        if (request.Method == HttpMethods.Get)
        {
            return RequestDataCollection.Load(requestPath, headers, query, Array.Empty<KeyValuePair<string, StringValues>>(), requestAuthorization);
        }
        else if (request.Method == HttpMethods.Post && request.HasFormContentType)
        {
            var forms = await request.ReadFormAsync(cancellationToken);

            return RequestDataCollection.Load(requestPath, headers, query, forms, requestAuthorization);
        }
        else
        {
            throw new Exception("Http method not support");
        }
    }
}
