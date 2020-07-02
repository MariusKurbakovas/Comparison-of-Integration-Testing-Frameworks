namespace DiscountService.Services.ServiceAgent.Contracts
{
    public class DiscountMetadataResponse
    {
        public decimal DiscountFromLoyalty { get; set; }

        public decimal? PercentDepositedInCard { get; set; }

        public decimal? CustomerDiscount { get; set; }

        public bool AreDiscountsSummed { get; set; }
    }
}