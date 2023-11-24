using System.Text.Json.Serialization;

namespace EasyIdentity.Models;

public class DeviceAuthorizationResultData
{
    [JsonPropertyName("device_code")]
    public string DeviceCode { get; set; } = null!;
    [JsonPropertyName("user_code")]
    public string UserCode { get; set; } = null!;
    [JsonPropertyName("verification_uri")]
    public string? VerificationUri { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("interval")]
    public int Interval { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("verification_uri_complete")]
    public bool? VerificationUriComplete { get; set; }
}