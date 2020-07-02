namespace DiscountMetadata.Services
{
    using System.IO;
    using Contracts;
    using Domain;
    using Mappers;
    using Newtonsoft.Json;

    public class MetadataService : IMetadataService
    {
        private readonly string _discountMetadatasFilePath;

        public MetadataService(string discountMetadatasFilePath)
        {
            _discountMetadatasFilePath = discountMetadatasFilePath;
        }

        public DiscountMetadataResponse GetDiscountMetadata(DiscountMetadataRequest request)
        {
            var discountMetadatasString = File.ReadAllText(_discountMetadatasFilePath);
            var discountMetadatas = JsonConvert.DeserializeObject<DiscountMetadatas>(discountMetadatasString);
            return discountMetadatas.MapToResponse(request);
        }
    }
}