using System.Security.Cryptography;

namespace EasyIdentity.SecurityKeys;

public class RsaSigningSecurityKey : SigningSecurityKey
{
    public RSA Rsa { get; }
    public override string Id { get; }
    public override string Algorithm { get; }

    public RsaSigningSecurityKey(RSA rsa, string algorithm, string id)
    {
        Rsa = rsa;
        Algorithm = algorithm;
        Id = id;
    }
}
