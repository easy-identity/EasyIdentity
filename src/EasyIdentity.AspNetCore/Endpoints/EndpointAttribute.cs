using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;

namespace EasyIdentity.Endpoints;

[AttributeUsage(AttributeTargets.Class)]
public class EndpointAttribute : Attribute
{
    public PathString Path { get; set; }

    public EndpointAttribute(string path)
    {
        Path = path;
    }

    public string Method { get; set; } = HttpMethods.Get;

    public static EndpointAttribute Get<T>()
    {
        var endpointAttribute = typeof(T).GetCustomAttribute<EndpointAttribute>();
        return endpointAttribute ?? throw new Exception($"The type '{typeof(T)}' [EndpointAttribute] missing");
    }
}
