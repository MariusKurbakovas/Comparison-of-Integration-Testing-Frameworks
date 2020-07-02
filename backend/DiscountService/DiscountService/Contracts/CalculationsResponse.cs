namespace DiscountService.Contracts
{
    using System;

    public class CalculationsResponse : CalculateDiscountRequest
    {
        public int Id { get; set; }

        public decimal DiscountedPrice { get; set; }

        public decimal? MoneySavedInCard { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal DiscountPercent { get; set; }
    }
}