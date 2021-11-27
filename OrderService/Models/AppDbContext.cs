using Microsoft.EntityFrameworkCore;

namespace OrderService.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderedProduct> OrderedProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }

}