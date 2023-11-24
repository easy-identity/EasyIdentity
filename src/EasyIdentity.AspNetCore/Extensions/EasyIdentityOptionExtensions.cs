using System.Reflection;
using EasyIdentity.Endpoints;
using EasyIdentity.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Endpoint = EasyIdentity.Endpoints.Endpoint;

namespace EasyIdentity.Extensions;

public static class EasyIdentityOptionExtensions
{
    public static EasyIdentityOptionBuilder AddEndpointHandler<T>(this EasyIdentityOptionBuilder builder, PathString path, string method = "GET") where T : class, IEndpointHandler
    {
        builder.Services.AddSingleton<IEndpoint>(new Endpoint(path, method, typeof(T)));
        builder.Services.AddScoped<IEndpointHandler, T>();
        builder.Services.AddScoped<T>();

        return builder;
    }

    public static EasyIdentityOptionBuilder AddEndpointHandler<T>(this EasyIdentityOptionBuilder builder) where T : class, IEndpointHandler
    {
        var endpointAttribute = typeof(T).GetCustomAttribute<EndpointAttribute>();
        return endpointAttribute == null
            ? throw new System.Exception($"The type '{typeof(T)}' [EndpointAttribute] missing")
            : AddEndpointHandler<T>(builder, endpointAttribute.Path, endpointAttribute.Method);
    }

    public static EasyIdentityOptionBuilder AddEndpointResultExecutor<TResult, TExecutor>(this EasyIdentityOptionBuilder builder)
        where TResult : class, IEndpointResult
        where TExecutor : class, IEndpointResultExecutor<TResult>
    {
        builder.Services.AddScoped<IEndpointResultExecutor<TResult>, TExecutor>();

        return builder;
    }
}
