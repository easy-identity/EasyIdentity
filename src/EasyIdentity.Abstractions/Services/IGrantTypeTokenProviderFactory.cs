namespace EasyIdentity.Services;

public interface IGrantTypeTokenProviderFactory
{
    IGrantTypeTokenProvider Get(string grantType);
}