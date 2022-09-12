using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        public HealthCheckController()
        {

        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
