namespace DiscountMetadata.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class MetadataController : ControllerBase
    {
        private readonly IMetadataService _metadataService;

        public MetadataController(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        [HttpGet]
        public DiscountMetadataResponse Get([FromQuery] DiscountMetadataRequest request)
        {
            return _metadataService.GetDiscountMetadata(request);
        }
    }
}