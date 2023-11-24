namespace EasyIdentity.Models;

public class DeviceCodeValidationResult : ValidationResult
{
    protected DeviceCodeValidationResult()
    {
    }

    public DeviceCodeValidationResult(DeviceCodeAuthorizedState state, string subject)
    {
        State = state;
        Subject = subject;
    }

    public DeviceCodeValidationResult(DeviceCodeAuthorizedState state)
    {
        State = state;
    }

    public DeviceCodeAuthorizedState State { get; }

    public string Subject { get; } = null!;
}
