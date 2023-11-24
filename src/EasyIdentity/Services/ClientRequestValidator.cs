using EasyIdentity.Constants;
using EasyIdentity.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class ClientRequestValidator : IClientRequestValidator
{
    protected ILogger<ClientRequestValidator> Logger { get; }
    protected IClientManager ClientManager { get; }
    protected IClientSecretValidator ClientSecretValidator { get; }
    protected IClientRedirectUrlValidator ClientRedirectUrlValidator { get; }

    public ClientRequestValidator(
        ILogger<ClientRequestValidator> logger,
        IClientManager clientManager,
        IClientSecretValidator clientSecretValidator,
        IClientRedirectUrlValidator clientRedirectUrlValidator)
    {
        Logger = logger;
        ClientManager = clientManager;
        ClientSecretValidator = clientSecretValidator;
        ClientRedirectUrlValidator = clientRedirectUrlValidator;
    }

    public virtual async Task<ClientValidationResult> ValidateAsync(string endpoint, string clientId, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Verify the client {ClientId} of {Endpoint} request", clientId, endpoint);

        var result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST, "Missing client_id parameter"));

        if (string.IsNullOrWhiteSpace(clientId))
        {
            result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST, "Missing client_id parameter"));
        }

        var client = await ClientManager.FindByIdAsync(clientId, cancellationToken);

        if (client == null)
        {
            result = ClientValidationResult.Error(Error.Create(Error.INVALID_CLIENT, "Invalid client or client not found."));
        }
        else
        {
            // scopes 
            if (requestData.Scopes?.Any() != true || client.Scopes.Except(requestData.Scopes).Any())
            {
                result = ClientValidationResult.Error(Error.Create(Error.INVALID_SCOPE));
            }

            bool oidcFlow = requestData.Scopes?.Contains("openid") == true;

            // scopes 
            if (requestData.ResponseTypes != null && (requestData.ResponseTypes.Contains("token") || requestData.ResponseTypes.Contains("id_token")) && requestData.Scopes?.Contains("openid") != true)
            {
                result = ClientValidationResult.Error(Error.Create(Error.INVALID_SCOPE));
            }

            //grant types
            var grantType = requestData.GrantType;

            if (!string.IsNullOrWhiteSpace(grantType))
            {
                if (!client.GrantTypes.Contains(grantType!))
                {
                    result = ClientValidationResult.Error(Error.Create(Error.UNSUPPORTED_GRANT_TYPE));
                }

                if (grantType == GrantTypes.AuthorizationCode && requestData.ResponseType != "code")
                {
                    result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
                }

                // seret
                if (client.Serets.Any())
                {
                    if (string.IsNullOrWhiteSpace(requestData.ClientSecret))
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.UNAUTHORIZED_CLIENT));
                    }
                    else if (!await ClientManager.CheckSecretAsync(client, requestData.ClientSecret, cancellationToken))
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.UNAUTHORIZED_CLIENT));
                    }
                }

                // redirect url
                if (grantType == GrantTypes.Implicit || grantType == GrantTypes.AuthorizationCode)
                {
                    if (!await ClientRedirectUrlValidator.ValidateAsync(client, requestData.RedirectUri, cancellationToken))
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
                    }

                    if (string.IsNullOrWhiteSpace(requestData.ResponseType))
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
                    }

                    // PKCE
                    var codeChallengeMethod = requestData.CodeChallengeMethod.ToString();
                    var codeChalleng = requestData.CodeChallenge.ToString();
                    if (!string.IsNullOrWhiteSpace(codeChallengeMethod) && codeChallengeMethod != "plain" && codeChallengeMethod != "S256" && string.IsNullOrWhiteSpace(codeChalleng))
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST, "code challenge params is invalid"));
                    }
                }

                // response types
                if (grantType == GrantTypes.Implicit)
                {
                    if (requestData.ResponseTypes?.Except(new[] { "code", "id_token", "token" }).Any() == true)
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
                    }

                    if (oidcFlow && requestData.Nonce == 0)
                    {
                        result = ClientValidationResult.Error(Error.Create(Error.INVALID_REQUEST));
                    }
                }
            }

            // 
            result = new ClientValidationResult(client);
        }

        if (result.Succeeded)
        {
            Logger.LogInformation("Verify the client '{clientId}' request is valid", clientId, result.Failure);
        }
        else
        {
            Logger.LogWarning("Verify the client '{clientId}' request is invalid. {Failure}", clientId, result.Failure);
        }

        return result;
    }
}
