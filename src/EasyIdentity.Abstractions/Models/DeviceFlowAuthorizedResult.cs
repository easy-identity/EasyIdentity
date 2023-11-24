namespace EasyIdentity.Services;

public class DeviceFlowAuthorizedResult
{
    public bool Valid { get; }

    public DeviceFlowAuthorizedResult(bool valid)
    {
        Valid = valid;
    }
}
