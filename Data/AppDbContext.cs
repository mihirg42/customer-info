using Microsoft.EntityFrameworkCore;
using customer_info.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customers_temp> Customers_temp { get; set; }
    public DbSet<Customers_Unit_temp> Customers_Unit_temp { get; set; }
}