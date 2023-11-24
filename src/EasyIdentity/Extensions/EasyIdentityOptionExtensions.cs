using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using EasyIdentity.Options;
using EasyIdentity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class EasyIdentityOptionExtensions
{
    public static EasyIdentityOptionBuilder AddCredentials(this EasyIdentityOptionBuilder builder, X509Certificate2 x509Certificate2)
    {
        builder.Services.AddSingleton<ICredentialsManager>(new X509CredentialsManager(x509Certificate2));

        return builder;
    }

#if !NETSTANDARD
    public static EasyIdentityOptionBuilder AddDevelopmentCredentials(this EasyIdentityOptionBuilder builder, bool persistence = true)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "eid-dev.pfx");

        if (File.Exists(path))
        {
            return AddCredentials(builder, new X509Certificate2(path));
        }

        var cert = CryptoHelper.CreateX509Certificate();

        if (persistence)
        {
            File.WriteAllBytes(path, cert.Export(X509ContentType.Pfx));
        }

        return AddCredentials(builder, cert);
    }
#endif

}
