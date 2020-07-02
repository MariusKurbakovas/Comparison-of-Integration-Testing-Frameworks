namespace DiscountService.Services.ServiceAgent
{
    using System.Threading.Tasks;
    using Contracts;

    public interface IDiscountMetadataServiceAgent
    {
        Task<DiscountMetadataResponse> GetDiscountMetadata(LoyaltyType loyaltyType, CustomerType customerType);
    }
}