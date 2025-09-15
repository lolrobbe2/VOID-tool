
using FoxholeBot.repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoxholeBot.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AssetsController : ControllerBase
    {
        [HttpGet("Carousel")]
        public IActionResult GetCarouselVideos()
        {
            return Ok(AssetRepository.GetCarouselVideos());
        }

        [HttpGet("Map")]
        public IActionResult GetMap()
        {
            return Ok(AssetRepository.GetMapImages());
        }
    }
}
