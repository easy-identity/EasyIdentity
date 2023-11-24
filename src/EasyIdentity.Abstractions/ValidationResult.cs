using EasyIdentity.Models;

namespace EasyIdentity;

public class ValidationResult
{
    public Error Failure { get; protected set; } = null!;

    public bool Succeeded => Failure == null;

    public static ValidationResult Error(Error error)
    {
        return new ValidationResult() { Failure = error };
    }
}
