namespace EasyIdentity.Models;

public class JwkKeyInfo
{
    public JwkKeyInfo(string kid, string use, string kty, string alg)
    {
        Kid = kid;
        Use = use;
        Kty = kty;
        Alg = alg;
    }

    /* common */
    public string Kid { get; set; }
    public string Use { get; set; }
    public string Kty { get; set; }
    public string Alg { get; set; }

    /* RSA */
    public string? N { get; set; }
    public string? E { get; set; }

    /* EC */
    public string? Crv { get; set; }
    public string? X { get; set; }
    public string? Y { get; set; }

    /* x509certificate */
    public string[]? X5c { get; set; }
    public string? X5t { get; set; }
}