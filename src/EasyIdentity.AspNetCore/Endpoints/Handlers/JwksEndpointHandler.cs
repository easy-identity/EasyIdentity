using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Endpoints.Results;
using EasyIdentity.Models;
using EasyIdentity.SecurityKeys;
using EasyIdentity.Services;

namespace EasyIdentity.Endpoints.Handlers;

[Endpoint("/.well-known/jwks")]
public class JwksEndpointHandler : EndpointHandler
{
    public override string Name => "jwks";

    protected ICredentialsManager CredentialsManager { get; }

    public JwksEndpointHandler(ICredentialsManager credentialsManager)
    {
        CredentialsManager = credentialsManager;
    }

    public override async Task<IEndpointResult> HandleAsync(IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var signingCredentials = await CredentialsManager.GetSigningCredentialsAsync(cancellationToken);
        var encryptingCredentials = await CredentialsManager.GetEncryptingCredentialsAsync(cancellationToken);

        var keys = new List<JwkKeyInfo>();

        foreach (var item in signingCredentials)
        {
            keys.Add(BuilderJwkKeyInfo(item, "sig"));
        }
        foreach (var item in encryptingCredentials)
        {
            keys.Add(BuilderJwkKeyInfo(item, "enc"));
        }

        return new JwksResult(keys);
    }

    protected JwkKeyInfo BuilderJwkKeyInfo(SecurityKey securityKey, string use)
    {
        if (securityKey is X509SigningSecurityKey x509Certificate)
        {
            var certificate = x509Certificate.Certificate;

            var cert64 = Convert.ToBase64String(certificate.RawData);
            var thumbprint = Base64Helper.Encode(certificate.GetCertHash());

            if (certificate.GetRSAPublicKey() is RSA rsa)
            {
                var parameters = rsa.ExportParameters(false);
                var exponent = Base64Helper.Encode(parameters.Exponent!);
                var modulus = Base64Helper.Encode(parameters.Modulus!);

                return new JwkKeyInfo(securityKey.Id, use, "RSA", x509Certificate.Algorithm)
                {
                    N = modulus,
                    E = exponent,
                    X5t = thumbprint,
                    X5c = new[] { cert64 },
                };
            }
            else if (certificate.GetECDsaPublicKey() is ECDsa ecdsa)
            {
                var parameters = ecdsa.ExportParameters(false);
                var x = Base64Helper.Encode(parameters.Q!.X!);
                var y = Base64Helper.Encode(parameters.Q!.Y!);

                return new JwkKeyInfo(securityKey.Id, use, "EC", x509Certificate.Algorithm)
                {
                    X = x,
                    Y = y,
                    Crv = CryptoHelper.GetCrvValueFromCurve(parameters.Curve),
                    X5t = thumbprint,
                    X5c = new[] { cert64 },
                };
            }
            else
            {
                throw new NotSupportedException($"key type '{certificate.PublicKey}' not supported.");
            }
        }
        else if (securityKey is RsaSigningSecurityKey rsaSecurityKey)
        {
            var parameters = rsaSecurityKey.Rsa.ExportParameters(false);

            return new JwkKeyInfo(rsaSecurityKey.Id, use, "RSA", rsaSecurityKey.Algorithm)
            {
                N = Convert.ToBase64String(parameters.Modulus!),
                E = Convert.ToBase64String(parameters.Exponent!),
            };
        }

        throw new NotSupportedException();
    }
}
