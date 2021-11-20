using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.SeedData;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>() ??
                      throw new NullReferenceException("Cannot get db context in while try to seed data");

        var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>() ??
                          throw new NullReferenceException("Cannot get db context in while try to seed data");

        SeedData(context, userManager, isProduction);
    }

    private static void SeedData(AppDbContext context, UserManager<IdentityUser> userManager, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("---> Atempting to apply migration ...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"---> Could not run migration: {e.Message}");
                Console.WriteLine(e);
            }
        }

        if (context.Users.Any())
        {
            Console.WriteLine("----> We already have data");
            return;
        }

        Console.WriteLine("----> Seed Data...");

        var admin = new IdentityUser { Email = "admin@coffeesystem.com", UserName = "Admin" };
        var client1 = new IdentityUser { Email = "client1@abc.com", UserName = "client1" };

        IdentityResult resul1 = userManager.CreateAsync(admin, "password").Result;
        IdentityResult result2 = userManager.CreateAsync(client1, "password").Result;
    }
}