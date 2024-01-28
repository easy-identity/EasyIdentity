using System.Text.Json.Serialization;

namespace EasyIdentity.Models;

public class TokenIntrospectionData
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("exp")]
    public int? ExpiresIn { get; set; }
}
