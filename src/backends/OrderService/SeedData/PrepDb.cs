using Microsoft.EntityFrameworkCore;
using OrderService.ExternalModels;
using OrderService.Models;

namespace OrderService.SeedData;
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
    }
}