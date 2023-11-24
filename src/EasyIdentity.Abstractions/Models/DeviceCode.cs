using System;

namespace EasyIdentity.Models;

public class DeviceFlowData
{
    public string ClientId { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string DeviceCode { get; set; } = null!;
    public string UserCode { get; set; } = null!;
    public string[] Scopes { get; set; } = null!;
    public bool IsAuthorized { get; set; }
    public bool IsReject { get; set; }
    public TimeSpan Lifetime { get; set; }
    public DateTimeOffset CreationTime { get; set; }
}
