namespace DiscountService.Persistence
{
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class DiscountDataContext : DbContext, IDiscountDataContext
    {
        public DiscountDataContext(DbContextOptions<DiscountDataContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<DiscountCalculation> DiscountCalculations { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscountCalculation>(discountCalculation =>
            {
                discountCalculation.HasKey(x => x.Id);
                discountCalculation.HasKey(x => x.Id);
                discountCalculation.Property(x => x.LoyaltyType).IsRequired();
                discountCalculation.Property(x => x.CustomerType).IsRequired();
                discountCalculation.Property(x => x.LoyaltyType).HasConversion<string>();
                discountCalculation.Property(x => x.CustomerType).HasConversion<string>();
                discountCalculation.Property(x => x.OriginalPrice).IsRequired();
                discountCalculation.Property(x => x.DiscountedPrice).IsRequired();
            });
        }
    }
}