using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Constants;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EasyIdentity.Services;

public class UserProvider<TUser> : IUserProvider where TUser : class
{
    protected UserManager<TUser> UserManager { get; }
    protected IUserClaimsPrincipalFactory<TUser> ClaimsFactory { get; set; }

    public UserProvider(UserManager<TUser> userManager, IUserClaimsPrincipalFactory<TUser> claimsFactory)
    {
        UserManager = userManager;
        ClaimsFactory = claimsFactory;
    }

    public async Task<List<Claim>> GetClaimsAsync(string subject, string[]? scopes, CancellationToken cancellationToken = default)
    {
        var user = await UserManager.FindByIdAsync(subject);

        if (user == null)
        {
            throw new Exception($"User with id '{subject}' not found.");
        }

        var claimsPrincipal = await ClaimsFactory.CreateAsync(user);

        var claims = claimsPrincipal.Claims.ToList();

        claims.RemoveAll(x => x.Type == UserManager.Options.ClaimsIdentity.SecurityStampClaimType);

        if (scopes?.Contains(StandardScopes.Email) != true)
        {
            claims.RemoveAll(x => x.Type == ClaimTypes.Email);
        }
        else if (await UserManager.IsEmailConfirmedAsync(user))
        {
            claims.Add(new Claim("email_verified", true.ToString().ToLowerInvariant(), ClaimValueTypes.Boolean));
        }

        if (scopes?.Contains(StandardScopes.Phone) == true)
        {
            var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                claims.Add(new Claim("phone_number", phoneNumber));
            }
            if (await UserManager.IsPhoneNumberConfirmedAsync(user))
            {
                claims.Add(new Claim("phone_number_verified", true.ToString().ToLowerInvariant(), ClaimValueTypes.Boolean));
            }
        }

        return claims;
    }

    public async Task<UserIdentityVerificationResult> ValidatePasswordAsync(string userName, string? password, IRequestCollection requestData, CancellationToken cancellationToken = default)
    {
        var user = await UserManager.FindByNameAsync(userName);

        if (user == null)
            return UserIdentityVerificationResult.Error(new Exception("User not found."));

        if (!await UserManager.CheckPasswordAsync(user, password!))
        {
            return UserIdentityVerificationResult.Error(new Exception("Invalid password."));
        }

        return new UserIdentityVerificationResult(await UserManager.GetUserIdAsync(user));
    }
}
