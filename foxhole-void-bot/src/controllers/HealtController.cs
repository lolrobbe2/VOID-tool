using Microsoft.AspNetCore.Mvc;

namespace FoxholeBot.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpHead]
        public IActionResult Get() => Ok(new { status = "ok" });
    }

}