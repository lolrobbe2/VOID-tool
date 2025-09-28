
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
        [HttpGet("Wind/Flag")]
        public IActionResult GetFlags()
        {
            return Ok(AssetRepository.GetFlagGifs());
        }
        [HttpGet("Wind/Sock")]
        public IActionResult GetWindSocks()
        {
            return Ok(AssetRepository.GetWindSockGifs());
        }
    }
}
