using Microsoft.AspNetCore.Mvc;
using FoxholeBot.repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
#nullable enable
namespace FoxholeBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockpileController : ControllerBase
    {
        private readonly StockpilesRepository _stockpileRepository;
        private readonly StockPileItemsRepository _stockpileItemRepository;


        public StockpileController(StockpilesRepository stockpileRepository, StockPileItemsRepository itemsRepository)
        {
            _stockpileRepository = stockpileRepository;
            _stockpileItemRepository = itemsRepository;
        }

        public class CreateStockpileRequest
        {
            public required string Name { get; set; }
            public required string Region { get; set; }
            public required string Subregion { get; set; }
            public  required string Code { get; set; }
        }

        /// <summary>
        /// Create a new stockpile.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStockpile([FromBody] CreateStockpileRequest request)
        {
            await _stockpileRepository.CreateStockPile(request.Name, request.Region, request.Subregion, request.Code);
            return CreatedAtAction(nameof(CreateStockpile), new { code = request.Code }, request);
        }

        /// <summary>
        /// Get stockpiles filtered by region.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetStockpilesByRegion([FromQuery] string? region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return Ok(await _stockpileRepository.GetAllStockpilesAsync());

            var stockpiles = await _stockpileRepository.GetRegionStockpiles(region);
            return Ok(stockpiles);
        }

        public class ItemsRequest
        {
            public required string Region { get; set; }
            public required string SubRegion { get; set; }
            public required string Name { get; set; }
            public required StockPileItem[] Items { get; set; }
        }

        [HttpPatch("items")]
        public async Task<IActionResult> AddStockpileItems([FromBody] ItemsRequest itemsRequest)
        {
            return AcceptedAtAction(nameof(AddStockpileItems), await _stockpileItemRepository.AddStockpileItems(itemsRequest.Region,itemsRequest.SubRegion,itemsRequest.Name,itemsRequest.Items));
        }
        [HttpDelete("items")]
        public async Task<IActionResult> RemoveStockpileItems([FromBody] ItemsRequest itemsRequest)
        {
            return AcceptedAtAction(nameof(RemoveStockpileItems), await _stockpileItemRepository.RemoveStockpileItems(itemsRequest.Region, itemsRequest.SubRegion, itemsRequest.Name, itemsRequest.Items));
        }
    }
}
