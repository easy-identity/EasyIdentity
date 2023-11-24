using System;
using System.Security.Cryptography.X509Certificates;

namespace EasyIdentity.SecurityKeys;

public class X509SigningSecurityKey : SigningSecurityKey
{
    public override string Id => Certificate.GetCertHashString();

    public X509Certificate2 Certificate { get; }
    public override string Algorithm { get; }

    public X509SigningSecurityKey(X509Certificate2 certificate, string algorithm)
    {
        Certificate = certificate;
        Algorithm = algorithm;
        if (!certificate.HasPrivateKey)
        {
            throw new ArgumentNullException(nameof(certificate), "Certificate has no private key");
        }
    }
}
