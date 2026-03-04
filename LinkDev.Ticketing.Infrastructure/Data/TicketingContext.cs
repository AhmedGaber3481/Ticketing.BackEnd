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
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingContext).Assembly);

            modelBuilder.Entity<Ticket>().ToTable("Ticket").HasKey(t => t.Id);
            modelBuilder.Entity<TicketAttachment>().ToTable("TicketAttachment").HasKey(t => t.Id);

            //modelBuilder.Entity<TicketCategory>().ToTable("TicketCategory").HasKey(t => new {t.Code, t.LangId});
            //modelBuilder.Entity<TicketPriority>().ToTable("TicketPriority").HasKey(t => new { t.Code, t.LangId });
            //modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus").HasKey(t => new { t.Code, t.LangId });
            //modelBuilder.Entity<TicketType>().ToTable("TicketType").HasKey(t => new { t.Code, t.LangId });

            modelBuilder.Entity<TicketCategory>().ToTable("TicketCategory").HasKey(t => t.Id);
            modelBuilder.Entity<TicketPriority>().ToTable("TicketPriority").HasKey(t => t.Id);
            modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus").HasKey(t => t.Id);
            modelBuilder.Entity<TicketType>().ToTable("TicketType").HasKey(t => t.Id);

            modelBuilder.Entity<TicketCategoryLookup>().ToView("TicketCategoryLookup").HasNoKey();
            modelBuilder.Entity<TicketStatusLookup>().ToView("TicketStatusLookup").HasNoKey();
            modelBuilder.Entity<TicketPriorityLookup>().ToView("TicketPriorityLookup").HasNoKey();
            modelBuilder.Entity<TicketTypeLookup>().ToView("TicketTypeLookup").HasNoKey();

            //modelBuilder.Entity<TicketSubCategory>().ToTable("TicketSubCategory").HasKey(t => new { t.Code, t.LangId });

            modelBuilder.Entity<TicketTransactionType>().ToTable("TicketTransactionType").HasKey(t => t.TypeId);
            modelBuilder.Entity<TicketTransactionStatus>().ToTable("TicketTransactionStatus").HasKey(t => t.StatusId);

            modelBuilder.Entity<TicketTransaction>().ToTable("TicketTransaction").HasKey(t => t.Id);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.Ticket).WithMany().HasForeignKey(e => e.TicketId);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.TicketTransactionStatus).WithMany().HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<TicketTransaction>().HasOne(e => e.TicketTransactionType).WithMany().HasForeignKey(e => e.TypeId);

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
