using ClientPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<Feature> Features { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>()
            .Property(e => e.Name).HasMaxLength(150);
        modelBuilder.Entity<Feature>()
            .Property(e => e.Description).HasMaxLength(2000);
        modelBuilder.Entity<Project>()
            .Property(e => e.Name).HasMaxLength(150);
        modelBuilder.Entity<Project>()
            .Property(e => e.Description).HasMaxLength(2000);
    }
}