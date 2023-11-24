using System.Security.Cryptography;

namespace EasyIdentity.SecurityKeys;

public class ECDsaSigningSecurityKey : SigningSecurityKey
{
    public ECDsa ECDsa { get; }
    public override string Algorithm { get; }
    public override string Id { get; }

    public ECDsaSigningSecurityKey(ECDsa eCDsa, string algorithm, string id)
    {
        ECDsa = eCDsa;
        Algorithm = algorithm;
        Id = id;
    }
}