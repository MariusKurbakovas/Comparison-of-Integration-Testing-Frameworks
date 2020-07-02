namespace DiscountService.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts;

    public interface IDiscountService
    {
        Task<List<CalculationsResponse>> GetAllCalculations();

        Task<CalculationsResponse> CalculateDiscount(CalculateDiscountRequest request);
    }
}