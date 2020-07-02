namespace DiscountMetadata.Mappers
{
    using Contracts;
    using Domain;

    public static class DiscountMetadataMapper
    {
        public static DiscountMetadataResponse MapToResponse(this DiscountMetadatas discountMetadatas, DiscountMetadataRequest request)
        {
            var discountMetadata = discountMetadatas.DiscountMetadataList[request.LoyaltyType];
            return new DiscountMetadataResponse
            {
                AreDiscountsSummed = discountMetadata.AreDiscountsSummed,
                DiscountFromLoyalty = discountMetadata.DiscountFromLoyalty,
                PercentDepositedInCard = discountMetadata.PercentDepositedInCard,
                CustomerDiscount = discountMetadata.CustomerDiscount[request.CustomerType]
            };
        }
    }
}