namespace EasyIdentity.Models;

public class AuthorizeResult
{
    public bool IsGranted { get; set; }
    public string[]? Scopes { get; set; }
}