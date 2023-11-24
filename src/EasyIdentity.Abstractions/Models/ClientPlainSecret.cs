namespace EasyIdentity.Models;

public class ClientPlainSecret : IClientSecret
{
    public string Secret { get; }

    public ClientPlainSecret(string secret)
    {
        Secret = secret;
    }
}
