using System;

namespace EasyIdentity.Models;

public class ClientToken
{
    public TimeSpan? AccessTokenLifetime { get; set; }
    public TimeSpan? IdTokenTokenLifetime { get; set; }
    public TimeSpan? RefreshTokenLifetime { get; set; }
    public TimeSpan? AuthorizationCodeLifetime { get; set; }
    public TimeSpan? DeviceCodeLifetime { get; set; }
}
