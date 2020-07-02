namespace DiscountService.Contracts
{
    using System.ComponentModel.DataAnnotations;

    public class CalculateDiscountRequest
    {
        [Required]
        public decimal OriginalPrice { get; set; }

        [Required]
        public LoyaltyType LoyaltyType { get; set; }

        [Required]
        public SpecialCustomers CustomerType { get; set; }

        public decimal? DiscountOnProduct { get; set; }
    }
}