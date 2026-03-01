using CorporatePortal.AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.AuthServer.Data;

public class ApplicationDbContext : IdentityDbContext<Collaborator>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        // Configure Collaborator (IdentityUser) table
        modelBuilder.Entity<Collaborator>().ToTable("AspNetUsers");
        
        // Configure additional properties
        modelBuilder.Entity<Collaborator>().Property(c => c.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Collaborator>().Property(c => c.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Collaborator>().Property(c => c.MiddleName)
            .HasColumnName("middle_name")
            .HasMaxLength(100)
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>().Property(c => c.BirthDate)
            .HasColumnName("birth_date")
            .HasColumnType("date")
            .IsRequired();

        modelBuilder.Entity<Collaborator>().Property(c => c.PositionId)
            .HasColumnName("position_id")
            .IsRequired();

        modelBuilder.Entity<Collaborator>().Property(c => c.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired();

        modelBuilder.Entity<Collaborator>().Property(c => c.WorkFormatId)
            .HasColumnName("work_format_id")
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>().Property(c => c.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        modelBuilder.Entity<Collaborator>().Property(c => c.RoleId)
            .HasColumnName("role_id")
            .IsRequired();

        modelBuilder.Entity<Collaborator>().Property(c => c.MobilePhone)
            .HasColumnName("mobile_phone")
            .HasMaxLength(20)
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>().Property(c => c.InternalPhone)
            .HasColumnName("internal_phone")
            .HasMaxLength(20)
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>().Property(c => c.Photo)
            .HasColumnName("photo")
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>().Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // Configure relationships
        modelBuilder.Entity<Collaborator>()
            .HasOne(c => c.Position)
            .WithMany(p => p.Collaborators)
            .HasForeignKey(c => c.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Collaborator>()
            .HasOne(c => c.Department)
            .WithMany(d => d.Collaborators)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Collaborator>()
            .HasOne(c => c.WorkFormat)
            .WithMany(w => w.Collaborators)
            .HasForeignKey(c => c.WorkFormatId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<Collaborator>()
            .HasOne(c => c.Organization)
            .WithMany(o => o.Collaborators)
            .HasForeignKey(c => c.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Collaborator>()
            .HasOne(c => c.Role)
            .WithMany(r => r.Collaborators)
            .HasForeignKey(c => c.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure other tables
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

