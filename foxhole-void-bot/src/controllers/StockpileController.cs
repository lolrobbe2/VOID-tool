using Microsoft.AspNetCore.Mvc;
using FoxholeBot.repositories;
using System.Threading.Tasks;
#nullable enable
namespace FoxholeBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockpileController : ControllerBase
    {
        private readonly StockpilesRepository _repository;

        public StockpileController(StockpilesRepository repository)
        {
            _repository = repository;
        }

        public class CreateStockpileRequest
        {
            public string Name { get; set; }
            public string Region { get; set; }
            public string Subregion { get; set; }
            public string Code { get; set; }
        }

        /// <summary>
        /// Create a new stockpile.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStockpile([FromBody] CreateStockpileRequest request)
        {
            await _repository.CreateStockPile(request.Name, request.Region, request.Subregion, request.Code);
            return CreatedAtAction(nameof(CreateStockpile), new { code = request.Code }, request);
        }

        /// <summary>
        /// Get stockpiles filtered by region.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetStockpilesByRegion([FromQuery] string? region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return Ok(await _repository.GetAllStockpilesAsync());

            var stockpiles = await _repository.GetRegionStockpiles(region);
            return Ok(stockpiles);
        }

    }
}
