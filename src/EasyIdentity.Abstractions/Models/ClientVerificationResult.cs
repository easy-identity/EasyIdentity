namespace EasyIdentity.Models;

public class ClientValidationResult : ValidationResult
{
    public Client Client { get; } = null!;

    protected ClientValidationResult()
    {
    }

    public ClientValidationResult(Client client)
    {
        Client = client;
    }

    public new static ClientValidationResult Error(Error error)
    {
        return new ClientValidationResult() { Failure = error };
    }
}