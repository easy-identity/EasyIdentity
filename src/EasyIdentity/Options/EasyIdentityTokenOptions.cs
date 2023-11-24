using System;

namespace EasyIdentity.Options;

public class EasyIdentityTokenOptions
{
    public TimeSpan DefaultAccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan DefaultRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);
    public TimeSpan DefaultIdentityTokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultDeviceCodeLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultAuthorizationCodeLifetime { get; set; } = TimeSpan.FromMinutes(5);
}
