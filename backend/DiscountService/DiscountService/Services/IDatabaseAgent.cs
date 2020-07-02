namespace DiscountService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface IDatabaseAgent
    {
        Task<DiscountCalculation> PersistDiscountCalculation(DiscountCalculation discountCalculation);

        Task<DiscountCalculation> GetDiscountCalculation(int id);

        Task<List<DiscountCalculation>> GetDiscountCalculations();

        Task DeleteDiscountCalculation(int id);

        Task SaveChanges();

        Task PurgeDiscountCalculations();
    }
}