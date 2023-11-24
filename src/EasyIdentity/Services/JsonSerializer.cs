using System.Text.Json;

#if !NET8_0_OR_GREATER
using EasyIdentity.Json;
#endif

namespace EasyIdentity.Services;

public class SystemTextJsonSerializer : IJsonSerializer
{
    private JsonSerializerOptions? _options;

    public virtual JsonSerializerOptions GetOptions()
    {
        if (_options != null)
            return _options;

#if NETSTANDARD
        _options = new JsonSerializerOptions();
#else
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
#endif

#if NET8_0_OR_GREATER
        _options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
#else
        _options.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
#endif 

#if !NETSTANDARD
        _options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
#endif

        return _options;
    }

    public virtual T? Deserialize<T>(string json)
    {
        var options = GetOptions();

        return System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
    }

    public virtual string? Serialize<T>(T value)
    {
        var options = GetOptions();

        return System.Text.Json.JsonSerializer.Serialize(value, options);
    }
}
