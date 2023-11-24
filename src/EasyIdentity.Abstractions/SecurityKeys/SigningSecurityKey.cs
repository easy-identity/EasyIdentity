namespace EasyIdentity.SecurityKeys;

public abstract class SigningSecurityKey : SecurityKey
{
    public abstract string Algorithm { get; }
}
