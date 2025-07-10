using EnsekTestTask.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace EnsekTestTask.Database;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Meter> Meters { get; set; }

    public ApplicationDbContext()
    {
        
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meter>()
            .HasOne(m => m.Account)
            .WithMany(a => a.Meters);
    }
}
