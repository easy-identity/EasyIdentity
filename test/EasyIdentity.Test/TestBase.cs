using EasyIdentity.Constants;
using EasyIdentity.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace EasyIdentity.Test;

public abstract class TestBase
{
    protected Client GetClient()
    {
        var client = new Client("client1", GrantTypes.All, Consts.AllStandardScopes);

        client.RedirectUrls.Add("https://sample1");
        client.RedirectUrls.Add("https://sample2");

        return client;
    }

    protected ClaimsPrincipal GetClaimsPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier,"a6ff920c-6829-4c1f-9fe1-72754e45aa93"),
            new Claim(ClaimTypes.Name, "bob"),
            new Claim(ClaimTypes.GivenName,"give name"),
            new Claim(ClaimTypes.Surname,"surname"),
        }));
    }
}