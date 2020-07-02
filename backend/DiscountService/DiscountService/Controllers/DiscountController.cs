namespace DiscountService.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IDatabaseAgent _databaseAgent;

        public DiscountController(IDiscountService discountService, IDatabaseAgent databaseAgent)
        {
            _discountService = discountService;
            _databaseAgent = databaseAgent;
        }

        [HttpGet]
        public async Task<IEnumerable<CalculationsResponse>> Get()
        {
            return await _discountService.GetAllCalculations();
        }

        [HttpPost]
        public async Task<CalculationsResponse> Post([Required] [FromBody] CalculateDiscountRequest request)
        {
            return await _discountService.CalculateDiscount(request);
        }

        [HttpPost]
        [Route("PurgeData")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task PurgeData()
        {
            await _databaseAgent.PurgeDiscountCalculations();
        }
    }
}