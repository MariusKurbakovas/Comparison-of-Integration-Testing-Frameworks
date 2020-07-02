namespace DiscountMetadata.Services
{
    using Contracts;

    public interface IMetadataService
    {
        DiscountMetadataResponse GetDiscountMetadata(DiscountMetadataRequest request);
    }
}