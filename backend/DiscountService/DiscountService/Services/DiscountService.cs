namespace DiscountService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Mappers;
    using ServiceAgent;
    using ServiceAgent.Contracts;
    using LoyaltyType = ServiceAgent.Contracts.LoyaltyType;

    public class DiscountService : IDiscountService
    {
        private readonly IDatabaseAgent _databaseAgent;
        private readonly IDiscountMetadataServiceAgent _discountMetadataServiceAgent;

        public DiscountService(IDatabaseAgent databaseAgent, IDiscountMetadataServiceAgent discountMetadataServiceAgent)
        {
            _databaseAgent = databaseAgent;
            _discountMetadataServiceAgent = discountMetadataServiceAgent;
        }

        public async Task<List<CalculationsResponse>> GetAllCalculations()
        {
            var calculations = await _databaseAgent.GetDiscountCalculations();
            return calculations.Select(x => x.MapToViewModel()).ToList();
        }

        public async Task<CalculationsResponse> CalculateDiscount(CalculateDiscountRequest request)
        {
            var metadata = await _discountMetadataServiceAgent.GetDiscountMetadata((LoyaltyType)request.LoyaltyType, (CustomerType)request.CustomerType);

            var discountPercent = CalculateDiscountPercent(request, metadata);
            var discountedPrice = CalculateDiscountedPrice(request, discountPercent);
            var moneySavedInCard = CalculateMoneySavedInCard(discountedPrice, metadata);

            var discountCalculation = request.MapToDomain(discountedPrice, moneySavedInCard, discountPercent);
            discountCalculation = await _databaseAgent.PersistDiscountCalculation(discountCalculation);
            return discountCalculation.MapToViewModel();
        }

        private static decimal CalculateDiscountPercent(CalculateDiscountRequest request, DiscountMetadataResponse metadata)
        {
            var percentOff = metadata.AreDiscountsSummed ?
                metadata.DiscountFromLoyalty + (metadata.CustomerDiscount ?? 0) + (request.DiscountOnProduct ?? 0) :
                Math.Max(Math.Max(metadata.DiscountFromLoyalty, metadata.CustomerDiscount ?? 0), request.DiscountOnProduct ?? 0);

            return percentOff >= 1 ? 0 : percentOff;
        }

        private static decimal CalculateDiscountedPrice(CalculateDiscountRequest request, decimal discountPercent)
        {
            var result = request.OriginalPrice - request.OriginalPrice * discountPercent;
            return result;
        }

        private static decimal? CalculateMoneySavedInCard(decimal discountedPrice, DiscountMetadataResponse metadata)
        {
            return discountedPrice * metadata.PercentDepositedInCard;
        }
    }
}