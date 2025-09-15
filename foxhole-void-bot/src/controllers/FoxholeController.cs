using FoxholeBot.repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoxholeBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoxholeController : ControllerBase
    {
        private readonly FoxholeRepository _repository;

        public FoxholeController(FoxholeRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("regions")]
        public IActionResult Get() => Ok(_repository.GetAllRegions());
    }

}