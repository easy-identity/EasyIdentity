using System;
using System.Text;

namespace EasyIdentity.Models;

public class RequestAuthorization
{
    public static RequestAuthorization Empty = new RequestAuthorization();

    protected RequestAuthorization()
    {
    }

    public RequestAuthorization(string schame, string parameter)
    {
        Schame = schame;
        Parameter = parameter;
    }

    public string Schame { get; } = null!;
    public string? Parameter { get; }

    public string[]? GetBasicAuth()
    {
        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Parameter ?? "")).Split(new char[] { ':' }, options: StringSplitOptions.RemoveEmptyEntries);
        }
        catch
        {
        }
        return null;
    }

    public bool IsBasicAuth() => Schame.Equals("basic", StringComparison.InvariantCultureIgnoreCase);
    public bool IsBearerAuth() => Schame.Equals("bearer", StringComparison.InvariantCultureIgnoreCase);
}