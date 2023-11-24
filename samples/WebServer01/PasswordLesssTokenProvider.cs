using EasyIdentity.Models;
using EasyIdentity.Services;

namespace WebServer01;

public class PasswordLessTokenProvider : IGrantTypeTokenProvider
{
    public string Name => "PasswordLess";

    public Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
