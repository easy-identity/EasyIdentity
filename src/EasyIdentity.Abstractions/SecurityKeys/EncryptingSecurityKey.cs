namespace EasyIdentity.SecurityKeys;

public abstract class EncryptingSecurityKey : SecurityKey
{
    public abstract string Algorithm { get; }
    public abstract string Encryption { get; }
}
