namespace DiscountService.Domain
{
    using System;

    public class DiscountCalculation
    {
        public int Id { get; set; }

        public decimal OriginalPrice { get; set; }

        public LoyaltyType LoyaltyType { get; set; }

        public CustomerType CustomerType { get; set; }

        public decimal? DiscountOnProduct { get; set; }

        public decimal DiscountedPrice { get; set; }

        public decimal? MoneySavedInCard { get; set; }

        public DateTime CreatonOn { get; set; }

        public decimal? DiscountPercent { get; set; }
    }
}