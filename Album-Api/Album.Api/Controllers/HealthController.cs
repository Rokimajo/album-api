using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers;

[Route("api")]
public class HealthController : Controller
{
    [HttpGet("health")]
    public IActionResult GetHealthCheck()
    {
        return Ok("API is running.");
    }
}