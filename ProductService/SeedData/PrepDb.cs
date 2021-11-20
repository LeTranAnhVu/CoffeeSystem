using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.SeedData;
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new NullReferenceException("Cannot get db context in while try to seed data");
        SeedData(context, isProduction);
    }

    private static void SeedData(AppDbContext context, bool isProduction)
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

        if (context.Products.Any())
        {
            Console.WriteLine("----> We already have data");
            return;
        }

        Console.WriteLine("----> Seed Data...");
        context.Products.AddRange(
            new Product { Name = "Capuchino" , Price = 5.9 },
            new Product { Name = "Latte" , Price = 4.7 },
            new Product { Name = "Flapino" , Price = 6.0 },
            new Product { Name = "Expresso" , Price = 2.3 }
        );

        context.SaveChanges();
    }
}