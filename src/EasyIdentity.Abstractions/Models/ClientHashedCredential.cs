using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyIdentity.Models;

public class ClientHashedSecret : IClientSecret
{
    public string Secret { get; }

    public ClientHashedSecret(string source)
    {
        Secret = Hash(source);
    }

    public static string Hash(string value)
    {
        using var md5 = MD5.Create();
        return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
}