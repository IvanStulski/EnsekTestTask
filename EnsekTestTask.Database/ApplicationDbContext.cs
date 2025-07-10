using EnsekTestTask.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace EnsekTestTask.Database;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Account> Accounts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
}
