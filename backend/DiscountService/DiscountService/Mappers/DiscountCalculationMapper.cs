namespace DiscountService.Mappers
{
    using System;
    using Contracts;
    using Domain;

    public static class DiscountCalculationMapper
    {
        public static DiscountCalculation MapToDomain(this CalculateDiscountRequest request, decimal discountedPrice, decimal? moneySavedInCard, decimal discountPercent)
        {
            return new DiscountCalculation
            {
                CustomerType = (CustomerType)request.CustomerType,
                OriginalPrice = request.OriginalPrice,
                LoyaltyType = (Domain.LoyaltyType)request.LoyaltyType,
                DiscountOnProduct = request.DiscountOnProduct,
                DiscountedPrice = discountedPrice,
                MoneySavedInCard = moneySavedInCard,
                CreatonOn = DateTime.Now,
                DiscountPercent = discountPercent
            };
        }

        public static CalculationsResponse MapToViewModel(this DiscountCalculation discountCalculation)
        {
            return new CalculationsResponse
            {
                LoyaltyType = (Contracts.LoyaltyType)discountCalculation.LoyaltyType,
                OriginalPrice = discountCalculation.OriginalPrice,
                CustomerType = (SpecialCustomers)discountCalculation.CustomerType,
                DiscountOnProduct = discountCalculation.DiscountOnProduct,
                Id = discountCalculation.Id,
                DiscountedPrice = discountCalculation.DiscountedPrice,
                MoneySavedInCard = discountCalculation.MoneySavedInCard,
                CreatedOn = discountCalculation.CreatonOn,
                DiscountPercent = discountCalculation.DiscountPercent ?? 0
            };
        }
    }
}