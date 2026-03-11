using LinkDev.Ticketing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Ticketing.Infrastructure.Data
{
    public class TicketingContext : DbContext
    {
        public TicketingContext(DbContextOptions<TicketingContext> contextOptions) : base(contextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasKey(t => t.Id);

            modelBuilder.Entity<TicketAttachment>().ToTable("TicketAttachment").HasKey(t => t.Id);

            modelBuilder.Entity<TicketCategory>().ToTable("TicketCategory").HasKey(t => t.Id);
            modelBuilder.Entity<TicketPriority>().ToTable("TicketPriority").HasKey(t => t.Id);
            modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus").HasKey(t => t.Id);
            modelBuilder.Entity<TicketType>().ToTable("TicketType").HasKey(t => t.Id);

            modelBuilder.Entity<TicketCategoryLookup>().ToView("TicketCategoryLookup").HasNoKey();
            modelBuilder.Entity<TicketStatusLookup>().ToView("TicketStatusLookup").HasNoKey();
            modelBuilder.Entity<TicketPriorityLookup>().ToView("TicketPriorityLookup").HasNoKey();
            modelBuilder.Entity<TicketTypeLookup>().ToView("TicketTypeLookup").HasNoKey();
            modelBuilder.Entity<ScalarInt>().HasNoKey();

            modelBuilder.Entity<TicketTransactionType>().ToTable("TicketTransactionType").HasKey(t => t.TypeId);
            modelBuilder.Entity<TicketTransactionStatus>().ToTable("TicketTransactionStatus").HasKey(t => t.StatusId);

            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasOne(e => e.TicketType).WithMany(e => e.Tickets).HasForeignKey(e => e.Type);
            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasOne(e => e.TicketStatus).WithMany(e => e.Tickets).HasForeignKey(e => e.Status);
            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasOne(e => e.TicketCategory).WithMany(e => e.Tickets).HasForeignKey(e => e.Category);
            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasOne(e => e.TicketPriority).WithMany(e => e.Tickets).HasForeignKey(e => e.Priority);

            modelBuilder.Entity<TicketTransaction>().ToTable("TicketTransaction").HasKey(t => t.Id);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.Ticket).WithMany().HasForeignKey(e => e.TicketId);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.TicketTransactionStatus).WithMany().HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.TicketTransactionType).WithMany().HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<TicketAttachment>().HasOne(e => e.Ticket).WithMany(e=> e.TicketAttachments).HasForeignKey(e => e.TicketId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading
            }
        }
        public DbSet<AppLanguage> AppLanguages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }

        //public DbSet<TicketSubCategory> TicketSubCategories { get; set; }
        public DbSet<TicketTransaction> TicketTransactions { get; set; }
        public DbSet<TicketTransactionStatus> TicketTransactionStatuses { get; set; }
        public DbSet<TicketTransactionType> TicketTransactionTypes { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
    }
}
