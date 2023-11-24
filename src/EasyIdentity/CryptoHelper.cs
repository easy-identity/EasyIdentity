using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EasyIdentity.Constants;

namespace EasyIdentity;

public static class CryptoHelper
{
    /// <summary>
    /// Return the matching RFC 7518 crv value for curve
    /// </summary>
    /// <param name="curve"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetCrvValueFromCurve(ECCurve curve)
    {
        return curve.Oid.Value switch
        {
            EcKeyConsts.CurveOids.P256 => JsonWebKeyConsts.ECTypes.P256,
            EcKeyConsts.CurveOids.P384 => JsonWebKeyConsts.ECTypes.P384,
            EcKeyConsts.CurveOids.P521 => JsonWebKeyConsts.ECTypes.P521,
            _ => throw new InvalidOperationException($"Unsupported curve type of {curve.Oid.Value} - {curve.Oid.FriendlyName}"),
        };
    }

    /// <summary>
    /// Creates the hash for the various hash claims (e.g. c_hash, at_hash or s_hash).
    /// </summary>
    /// <param name="value">The value to hash.</param>
    /// <param name="tokenSigningAlgorithm">The token signing algorithm</param>
    public static string CreateHashClaimValue(string value, string tokenSigningAlgorithm)
    {
        using (var sha = GetHashAlgorithmForSigningAlgorithm(tokenSigningAlgorithm))
        {
            var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(value));
            var size = sha.HashSize / 8 / 2;

            var leftPart = new byte[size];
            Array.Copy(hash, leftPart, size);

            return Base64Helper.Encode(leftPart);
        }
    }

    /// <summary>
    /// Returns the matching hashing algorithm for a token signing algorithm
    /// </summary>
    /// <param name="signingAlgorithm">The signing algorithm</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static HashAlgorithm GetHashAlgorithmForSigningAlgorithm(string signingAlgorithm)
    {
        var signingAlgorithmBits = int.Parse(signingAlgorithm.Substring(signingAlgorithm.Length - 3));

        return signingAlgorithmBits switch
        {
            256 => SHA256.Create(),
            384 => SHA384.Create(),
            512 => SHA512.Create(),
            _ => throw new InvalidOperationException($"Invalid signing algorithm: {signingAlgorithm}"),
        };
    }

#if !NETSTANDARD
    public static X509Certificate2 CreateX509Certificate(RSA? rSA = null)
    {
        rSA ??= RSA.Create();

        var request = new CertificateRequest("cn=develop", rSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(50));
    }

#endif
}
