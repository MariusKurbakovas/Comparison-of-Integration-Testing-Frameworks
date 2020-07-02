namespace DiscountService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class DatabaseAgent : IDatabaseAgent
    {
        private readonly IDiscountDataContext _dataContext;

        public DatabaseAgent(IDiscountDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<DiscountCalculation> PersistDiscountCalculation(DiscountCalculation discountCalculation)
        {
            var entity = _dataContext.DiscountCalculations.Add(discountCalculation);
            await SaveChanges();
            return entity.Entity;
        }

        public Task<DiscountCalculation> GetDiscountCalculation(int id)
        {
            return _dataContext.DiscountCalculations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<DiscountCalculation>> GetDiscountCalculations()
        {
            return _dataContext.DiscountCalculations.ToListAsync();
        }

        public async Task DeleteDiscountCalculation(int id)
        {
            var entity = await GetDiscountCalculation(id);
            _dataContext.DiscountCalculations.Remove(entity);
            await SaveChanges();
        }

        public Task SaveChanges()
        {
            return _dataContext.SaveChangesAsync();
        }

        public Task PurgeDiscountCalculations()
        {
            _dataContext.DiscountCalculations.RemoveRange(_dataContext.DiscountCalculations);
            return SaveChanges();
        }
    }
}