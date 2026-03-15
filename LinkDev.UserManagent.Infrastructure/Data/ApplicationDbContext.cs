using LinkDev.UserManagent.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.UserManagent.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

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
    }
}
