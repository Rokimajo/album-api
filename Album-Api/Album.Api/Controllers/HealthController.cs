using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers;

[Route("api")]
public class HealthController : Controller
{
    [HttpGet("health")]
    public IActionResult GetHealthCheck()
    {
        try
        {
            Console.WriteLine($"{DateTime.Now} | GET /health called.");
            var response = "API is running.";
            Console.WriteLine($"{DateTime.Now} | GET /health response: {response}");
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now} | Error occurred inside GET /health controller, returning HTTP Status 500");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}