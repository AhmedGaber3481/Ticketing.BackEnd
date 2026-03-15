using LinkDev.UserManagent.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LinkDev.UserManagent.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<UserDetails>().ToTable("UserDetails").HasKey(e => e.UserId);
        //    builder.Entity<AspNetUsers>().ToTable("AspNetUsers").HasKey(e => e.Id);
        //    builder.Entity<AspNetUserRoles>().ToTable("AspNetUserRoles").HasKey(e =>  new { e.UserId, e.RoleId });
        //    builder.Entity<AspNetRoles>().ToTable("AspNetRoles").HasKey(e => e.Id);

        //    //builder.Entity<AspNetUserRoles>().HasOne(e => e.User).WithMany(e=> e.UserRoles).HasForeignKey(e => e.UserId);
        //    //builder.Entity<AspNetUserRoles>().HasOne(e => e.Role).WithMany(e=> e.UserRoles).HasForeignKey(e => e.RoleId);

        //    builder.Entity<UserDetails>().HasOne(e => e.User).WithOne(e => e.UserDetails).HasForeignKey<UserDetails>(e => e.UserId);

        //    base.OnModelCreating(builder);
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserDetails>()
                .ToTable("UserDetails")
                .HasKey(e => e.UserId);

            builder.Entity<UserDetails>()
                .HasOne(e => e.User)
                .WithOne(e => e.UserDetails)
                .HasForeignKey<UserDetails>(e => e.UserId);
        }
        public DbSet<UserDetails> UserDetails { get; set; }

        //public DbSet<AspNetUsers> UsersSet { get; set; }
        //public DbSet<AspNetUserRoles> UserRolesSet { get; set; }
        //public DbSet<AspNetRoles> RolesSet { get; set; }
    }
}
