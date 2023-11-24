using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

/// <summary>
///  Token creation provder by grant type
/// </summary>
public interface IGrantTypeTokenProvider
{
    /// <summary>
    ///  grant type name
    /// </summary>
    string Name { get; }

    /// <summary>
    ///  Validation the token request
    /// </summary>
    /// <param name="client"></param>
    /// <param name="requestData"></param>
    /// <param name="cancellationToken"></param>
    Task<TokenRequestValidationResult> ValidateAsync(Client client, IRequestCollection requestData, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Create token
    /// </summary>
    /// <param name="client"></param>
    /// <param name="tokenRequestValidation"></param>
    /// <param name="requestData"></param>
    /// <param name="cancellationToken"></param>
    Task<TokenGenerated> CreateTokenAsync(Client client, TokenRequestValidationResult tokenRequestValidation, IRequestCollection requestData, CancellationToken cancellationToken = default);
}
