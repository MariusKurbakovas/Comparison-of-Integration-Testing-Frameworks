namespace DiscountService.Persistence
{
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IDiscountDataContext
    {
        DbSet<DiscountCalculation> DiscountCalculations { get; set; }

        Task<int> SaveChangesAsync();
    }
}