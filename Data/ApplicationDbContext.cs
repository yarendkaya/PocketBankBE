using Microsoft.EntityFrameworkCore;
using PocketBankBE.Models;

namespace PocketBankBE.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.Property(u => u.Balance)
                .HasPrecision(15, 2);
        });
    }
}