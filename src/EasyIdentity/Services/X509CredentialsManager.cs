using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.SecurityKeys;

namespace EasyIdentity.Services;

public class X509CredentialsManager : ICredentialsManager
{
    private X509Certificate2 X509Certificate { get; }

    private readonly string _signingAlgorithm;
    private readonly string _keyWrapAlgorithm;
    private readonly string _dataEncryptionAlgorithm;

    public X509CredentialsManager(X509Certificate2 x509Certificate) : this(x509Certificate, AlgorithmConsts.RsaSigningAlgorithm_RS256, "RSA-OAEP", "A128CBC-HS256")
    {
    }

    public X509CredentialsManager(X509Certificate2 x509Certificate, string signingAlgorithm, string keyWrapAlgorithm, string dataEncryptionAlgorithm)
    {
        X509Certificate = x509Certificate;
        _signingAlgorithm = signingAlgorithm;
        _keyWrapAlgorithm = keyWrapAlgorithm;
        _dataEncryptionAlgorithm = dataEncryptionAlgorithm;
    }

    public virtual Task<List<EncryptingSecurityKey>> GetEncryptingCredentialsAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<EncryptingSecurityKey>();

        return Task.FromResult(list);
    }

    public virtual Task<List<SigningSecurityKey>> GetSigningCredentialsAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<SigningSecurityKey>
        {
            new X509SigningSecurityKey(X509Certificate, _signingAlgorithm)
        };

        return Task.FromResult(list);
    }
}
