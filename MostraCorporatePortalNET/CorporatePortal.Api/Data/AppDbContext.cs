using Microsoft.EntityFrameworkCore;
using CorporatePortal.Api.Models;

namespace CorporatePortal.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<WorkFormat> WorkFormats => Set<WorkFormat>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure tables
        modelBuilder.Entity<Position>().ToTable("positions");
        modelBuilder.Entity<Position>().Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        modelBuilder.Entity<Position>().Property(p => p.Name).HasColumnName("name").IsRequired();

        modelBuilder.Entity<Department>().ToTable("departments");
        modelBuilder.Entity<Department>().Property(d => d.Id).HasColumnName("id").ValueGeneratedOnAdd();
        modelBuilder.Entity<Department>().Property(d => d.Name).HasColumnName("name").IsRequired();

        modelBuilder.Entity<WorkFormat>().ToTable("workformats");
        modelBuilder.Entity<WorkFormat>().Property(w => w.Id).HasColumnName("id").ValueGeneratedOnAdd();
        modelBuilder.Entity<WorkFormat>().Property(w => w.Name).HasColumnName("name").IsRequired();

        modelBuilder.Entity<Organization>().ToTable("organizations");
        modelBuilder.Entity<Organization>().Property(o => o.Id).HasColumnName("id").ValueGeneratedOnAdd();
        modelBuilder.Entity<Organization>().Property(o => o.Name).HasColumnName("name").IsRequired();

        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Role>().Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().Property(r => r.Name).HasColumnName("name").IsRequired();
    }
}

