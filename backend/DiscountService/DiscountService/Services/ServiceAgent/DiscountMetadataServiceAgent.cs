namespace DiscountService.Services.ServiceAgent
{
    using System.Threading.Tasks;
    using Contracts;
    using RestSharp;

    public class DiscountMetadataServiceAgent : IDiscountMetadataServiceAgent
    {
        private readonly IRestClient _client;

        public DiscountMetadataServiceAgent(IRestClient client)
        {
            _client = client;
        }

        public Task<DiscountMetadataResponse> GetDiscountMetadata(LoyaltyType loyaltyType, CustomerType customerType)
        {
            var request = new RestRequest($"/metadata?loyaltyType={loyaltyType}&customerType={customerType}", Method.GET);
            return _client.GetAsync<DiscountMetadataResponse>(request);
        }
    }
}