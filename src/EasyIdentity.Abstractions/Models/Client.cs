using System.Collections.Generic;

namespace EasyIdentity.Models;

public class Client
{
    public Client()
    {
    }

    public Client(string name, List<string> grantTypes, List<string> scopes)
    {
        Name = name;
        GrantTypes = grantTypes;
        Scopes = scopes;
    }

    public string Name { get; set; } = null!;
    public List<string> GrantTypes { get; set; } = [];
    public List<string> Scopes { get; } = [];
    public List<string> RedirectUrls { get; } = [];
    public ClientToken Token { get; set; } = new ClientToken();
    public List<IClientSecret> Serets { get; } = [];

    public override string ToString()
    {
        return Name;
    }
}
