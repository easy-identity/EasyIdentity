//namespace EasyIdentity.Models;

//[]
//public class DeviceCodeAuthorizedResult
//{
//    private static readonly DeviceCodeAuthorizedResult _invalid = new DeviceCodeAuthorizedResult { IsInvalid = true };
//    private static readonly DeviceCodeAuthorizedResult _expired = new DeviceCodeAuthorizedResult { IsInvalid = true };
//    private static readonly DeviceCodeAuthorizedResult _granted = new DeviceCodeAuthorizedResult { IsInvalid = true };

//    public bool IsInvalid { get; protected set; }
//    public bool IsExpired { get; protected set; }
//    public bool IsGranted { get; protected set; }

//    public static DeviceCodeAuthorizedResult Invalid { get; } = _invalid;
//    public static DeviceCodeAuthorizedResult Expired { get; } = _expired;
//    public static DeviceCodeAuthorizedResult Granted { get; } = _granted;
//}