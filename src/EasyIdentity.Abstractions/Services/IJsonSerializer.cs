using System.Text.Json;

namespace EasyIdentity.Services;

public interface IJsonSerializer
{
    JsonSerializerOptions GetOptions();

    string? Serialize<T>(T value);

    T? Deserialize<T>(string json);
}
