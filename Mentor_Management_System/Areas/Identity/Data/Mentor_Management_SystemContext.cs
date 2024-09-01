using Mentor_Management_System.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentor_Management_System.Areas.Identity.Data;

public class Mentor_Management_SystemContext : IdentityDbContext<Mentor_Management_SystemUser, IdentityRole, string>
{
    
    public Mentor_Management_SystemContext(DbContextOptions<Mentor_Management_SystemContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" });

        builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" });

        builder.ApplyConfiguration(new EntityConfiguration());
        builder.ApplyConfiguration(new EntityRoleConfiguration());
    }
}

public class EntityConfiguration : IEntityTypeConfiguration<Mentor_Management_SystemUser>
{
    public void Configure(EntityTypeBuilder<Mentor_Management_SystemUser> builder)
    {
        builder.Property(Fname => Fname.FirstName).HasMaxLength(255);
        builder.Property(Lname =>  Lname.LastName).HasMaxLength(255);
        builder.Property(phone => phone.phone).HasMaxLength(255);
    }
}

public class EntityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {

    }
}




