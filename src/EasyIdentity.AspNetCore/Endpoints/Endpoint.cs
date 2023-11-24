using System;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public class Endpoint : IEndpoint
{
    public Endpoint(PathString path, string method, Type type)
    {
        Path = path;
        Method = method;
        Type = type;
    }

    public PathString Path { get; }
    public string Method { get; }
    public Type Type { get; }
}