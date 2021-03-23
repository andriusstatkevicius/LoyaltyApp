using LoyaltyLibrary;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyAppData
{
    public class LoyaltyAppContext : DbContext
    {
        public LoyaltyAppContext(DbContextOptions<LoyaltyAppContext> options) : base(options)
        {
            // Enhancing performance where tracking the changes is not needed
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<DiscountCard> LoyaltyCards { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyBalanceTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasIndex(c => new
            {
                c.Name,
                c.Surname,
                c.Email
            }).IsUnique();

            modelBuilder.Entity<DiscountCard>().HasKey(card => new { card.Number });

            modelBuilder.Entity<DiscountCard>().HasOne(card => card.Client)
                                               .WithMany(client => client.DiscountCards);

            modelBuilder.Entity<LoyaltyTransaction>().HasOne(transaction => transaction.Card)
                                                     .WithMany(card => card.LoyaltyTransactions);

            modelBuilder.Entity<LoyaltyTransaction>().HasOne(transaction => transaction.Client)
                                                     .WithMany().OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
