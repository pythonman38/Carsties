using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any()) return;

        var destiny = userMgr.FindByNameAsync("destiny").Result;
        if (destiny == null)
        {
            destiny = new ApplicationUser
            {
                UserName = "destiny",
                Email = "DestinyWalker@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(destiny, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(destiny, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Destiny Walker"),
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("destiny created");
        }
        else
        {
            Log.Debug("destiny already exists");
        }

        var michael = userMgr.FindByNameAsync("michael").Result;
        if (michael == null)
        {
            michael = new ApplicationUser
            {
                UserName = "michael",
                Email = "MichaelWalker@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(michael, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(michael, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Michael Walker"),
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("michael created");
        }
        else
        {
            Log.Debug("michael already exists");
        }
    }
}
