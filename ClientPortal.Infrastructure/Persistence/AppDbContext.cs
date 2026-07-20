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
    public DbSet<User> Users { get; set; }
    public DbSet<MagicLink> MagicLinks { get; set; }

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
        
        modelBuilder.Entity<User>()
            .Property(e => e.Email).HasMaxLength(256).IsRequired();
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email).IsUnique();
        
        modelBuilder.Entity<MagicLink>()
            .Property(e => e.TokenHash)
            .HasMaxLength(64)
            .IsFixedLength()
            .IsRequired();
        modelBuilder.Entity<MagicLink>()
            .HasIndex(e => e.TokenHash)
            .IsUnique();
        modelBuilder.Entity<MagicLink>()
            .Property(e => e.Email).HasMaxLength(256).IsRequired();
    }
}