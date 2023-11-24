using IdentityModel.Client;

namespace ClientApp01;

public static class Test
{
    public static async Task<TokenResponse> RefreshTokenAsync(HttpClient httpClient, DiscoveryDocumentResponse discovery, string refreshToken, string scope)
    {
        Console.WriteLine("Request refresh token ");
        var result = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
        {
            Address = discovery.TokenEndpoint,
            ClientId = "client1",
            ClientSecret = "",
            Scope = scope,
            RefreshToken = refreshToken,
        });

        CheckTokenResponse(result);

        return result;
    }
    public static async Task<TokenResponse> ClientAsync(HttpClient httpClient, DiscoveryDocumentResponse discovery)
    {
        var result = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        {
            Address = discovery.TokenEndpoint,
            ClientId = "client1",
            Scope = "demo",
        });

        CheckTokenResponse(result);

        return result;
    }

    public static async Task<TokenResponse> PasswordAsync(HttpClient httpClient, DiscoveryDocumentResponse discovery)
    {
        var result = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = discovery.TokenEndpoint,
            ClientId = "client1",
            Scope = "demo email openid",
            UserName = "bob",
            Password = "P@ssw0rd",
        });

        CheckTokenResponse(result);

        if (!string.IsNullOrWhiteSpace(result.RefreshToken))
        {
            await RefreshTokenAsync(httpClient, discovery, result.RefreshToken, result.Scope!);
        }

        return result;
    }

    public static async Task<TokenResponse?> DeviceAsync(HttpClient httpClient, DiscoveryDocumentResponse discovery)
    {
        var deviceResult = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
        {
            Address = discovery.DeviceAuthorizationEndpoint,

            ClientId = "client1",
            ClientSecret = "",
            Scope = "demo email",
        });

        if (deviceResult.IsError)
        {
            throw new Exception(deviceResult.Error);
        }

        Console.WriteLine(deviceResult.Json);

        Console.WriteLine("Please open browser and enter user code");

        TokenResponse? result;
        do
        {
            result = await httpClient.RequestDeviceTokenAsync(new DeviceTokenRequest()
            {
                Address = discovery.TokenEndpoint,

                ClientId = "client1",

                DeviceCode = deviceResult.DeviceCode!,
            });

            Console.WriteLine(result.Json);

            if (result.IsError)
            {
                Thread.Sleep(deviceResult.Interval * 1000);
            }
        } while (result.IsError && result.Error == "authorization_pending");

        if (!string.IsNullOrWhiteSpace(result.RefreshToken))
        {
            await RefreshTokenAsync(httpClient, discovery, result.RefreshToken, result.Scope!);
        }

        return result;
    }

    private static void CheckTokenResponse(TokenResponse tokenResponse)
    {
        if (tokenResponse.IsError)
        {
            throw new Exception(tokenResponse.Error);
        }

        Console.WriteLine(tokenResponse.Raw);
    }
}
