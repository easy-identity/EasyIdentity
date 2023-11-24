using EasyIdentity.Constants;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebServer01;
using WebServer01.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.Password.RequireNonAlphanumeric = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.AddEasyIdentity()
    .AddDevelopmentCredentials()
    .AddIdentityExtension<IdentityUser>()
    .AddTokenHandler<PasswordLessTokenProvider>()
    .AddStaticStore(store =>
    {
        store.CustomScopes("scope1", "scope2", "demo")
            .Clients(new Client(
                name: "client1",
                grantTypes: new List<string>
                {
                    GrantTypes.ClientCredentials,
                    GrantTypes.Password,
                    GrantTypes.AuthorizationCode,
                    GrantTypes.Implicit,
                    GrantTypes.DeviceCode
                },
                scopes: new List<string> { "demo", "email" })
            {
                RedirectUrls = { "https://oauth.pstmn.io/v1/callback", "http://127.0.0.1:12345" }
            });
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    if (await userManager.FindByNameAsync("bob") == null)
    {
        var result = await userManager.CreateAsync(new IdentityUser("bob")
        {
            Email = "bob@demo.com",
            EmailConfirmed = true,
            PhoneNumber = "+000000000",
            PhoneNumberConfirmed = true,
        }, "P@ssw0rd");

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.First().Description);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseEasyIdentity();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
