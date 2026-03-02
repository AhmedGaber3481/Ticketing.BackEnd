using LinkDev.UserManagent.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.UserManagent.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserDetails>().ToTable("UserDetails").HasKey(e => e.UserId);

            base.OnModelCreating(builder);
        }
        public DbSet<UserDetails> UserDetails { get; set; }
    }
}
