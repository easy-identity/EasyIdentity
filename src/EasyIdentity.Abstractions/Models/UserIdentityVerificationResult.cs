using System;

namespace EasyIdentity.Models;

public class UserIdentityVerificationResult
{
    public Exception? Fail { get; set; }
    public bool Succeeded => Fail == null;

    public string Subject { get; } = null!;

    protected UserIdentityVerificationResult()
    {
    }

    public UserIdentityVerificationResult(string subject)
    {
        Subject = subject;
    }

    public static UserIdentityVerificationResult Error(Exception exception)
    {
        return new UserIdentityVerificationResult() { Fail = exception };
    }
}