using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers;

[Route("api")]
public class HelloController : Controller
{
    private readonly IGreeting gService;

    public HelloController(IGreeting greeting, ILogger<HelloController> logger)
    {
        gService = greeting;
    }
    
    [HttpGet("hello")]
    public async Task<IActionResult> GetHello([FromQuery] string name)
    {
        Console.WriteLine("GET /hello called.");
        var returnString = await gService.ReturnHello(name);
        return Ok(new Model() {Response = returnString});
    }
}