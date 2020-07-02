namespace DiscountMetadata.Domain
{
    using System.Collections.Generic;
    using Contracts;

    public class DiscountMetadatas
    {
        public Dictionary<LoyaltyType, DiscountMetadata> DiscountMetadataList { get; set; }
    }
}