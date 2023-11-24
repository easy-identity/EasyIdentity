// See https://aka.ms/new-console-template for more information
using IdentityModel.OidcClient;
using Microsoft.Net.Http.Server;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

Console.WriteLine("Hello, World!");

var scope = "openid email offline_access";

OidcClient client = new OidcClient(new OidcClientOptions
{
    Authority = "https://localhost:44396/",
    ClientId = "client1",
    ClientSecret = "",
    Scope = scope,
    LoadProfile = false,
    FilterClaims = true,
    RedirectUri = "http://127.0.0.1:12345",
});

var state = await client.PrepareLoginAsync();

Console.WriteLine($"Try load url {state.StartUrl}");

OpenBrowser(state.StartUrl);

// create a redirect URI using an available port on the loopback address.
string redirectUri = string.Format("http://127.0.0.1:12345");

// create an HttpListener to listen for requests on that redirect URI.
var settings = new WebListenerSettings();
settings.UrlPrefixes.Add(redirectUri);
var http = new WebListener(settings);

http.Start();
Console.WriteLine("Listening..");

var context = await http.AcceptAsync();

context.Response.ContentType = "text/plain";
context.Response.StatusCode = 200;

var buffer = Encoding.UTF8.GetBytes("Login completed. Please return to the console application.");
await context.Response.Body.WriteAsync(buffer);
await context.Response.Body.FlushAsync();

context.Dispose();

var result = await client.ProcessResponseAsync(context.Request.QueryString, state);

if (result.IsError)
{
    Console.WriteLine("An error occurred: {0}", result.Error);
}
else
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Claims:");

    foreach (var claim in result.User.Claims)
    {
        Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
    }

    Console.WriteLine();
    Console.WriteLine("Access token:");
    Console.WriteLine();
    Console.WriteLine(result.AccessToken);

    Console.WriteLine();
    Console.WriteLine("Request user info ...");

    var userInfo = await client.GetUserInfoAsync(result.AccessToken);
    if (userInfo.IsError)
    {
        Console.WriteLine("An error occurred: {0}", userInfo.Error);
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Claims:");
        foreach (var claim in result.User.Claims)
        {
            Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
        }

        Console.WriteLine();
    }

    if (!string.IsNullOrWhiteSpace(result.RefreshToken))
    {
        Console.WriteLine();
        Console.WriteLine("Refresh token:");
        Console.WriteLine();
        Console.WriteLine(result.RefreshToken);

        Console.WriteLine();
        Console.WriteLine("Request refresh token ...");
        var refreshTokenResult = await client.RefreshTokenAsync(result.RefreshToken, scope: scope);
        if (refreshTokenResult.IsError)
        {
            Console.WriteLine("An error occurred: {0}", refreshTokenResult.Error);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Access token:");
            Console.WriteLine();
            Console.WriteLine(result.AccessToken);
        }
    }
}

static void OpenBrowser(string url)
{
    try
    {
        Process.Start(url);
    }
    catch
    {
        // hack because of this: https://github.com/dotnet/corefx/issues/10361
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            throw;
        }
    }
}