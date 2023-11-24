// See https://aka.ms/new-console-template for more information
using ClientApp01;
using IdentityModel.Client;

Console.WriteLine("Hello, World!");

var serverUrl = "https://localhost:44396";

var httpClient = new HttpClient();

var discovery = await httpClient.GetDiscoveryDocumentAsync(serverUrl);

if (discovery.IsError) throw new Exception(discovery.Error);

Console.WriteLine("Request client token ");
await Test.ClientAsync(httpClient, discovery);

Console.WriteLine("Request password token ");
await Test.PasswordAsync(httpClient, discovery);

//Console.WriteLine("Request device token ");
//await Test.DeviceAsync(httpClient, discovery);