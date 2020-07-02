namespace DiscountMetadata.Domain
{
    using System.Collections.Generic;
    using Contracts;

    public class DiscountMetadata
    {
        public Dictionary<CustomerType, decimal?> CustomerDiscount { get; set; }

        public decimal? PercentDepositedInCard { get; set; }

        public decimal DiscountFromLoyalty { get; set; }

        public bool AreDiscountsSummed { get; set; }
    }
}