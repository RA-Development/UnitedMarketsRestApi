using Microsoft.AspNetCore.Mvc;
using UnitedMarkets.Core.ApplicationServices;

namespace UnitedMarkets.UI.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketsController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_marketService.GetAll());
        }
    }
}
