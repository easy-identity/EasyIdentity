using System;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public interface IEndpoint
{
    PathString Path { get; }

    string Method { get; }

    Type Type { get; }
}
