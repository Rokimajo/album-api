using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers;

[Route("api")]
public class HelloController : Controller
{
    private readonly IGreeting gService;
    private readonly FakeGreetingService fakeService;

    public HelloController(IGreeting greeting, FakeGreetingService fakeGreeting)
    {
        gService = greeting;
        fakeService = fakeGreeting;
    }
    
    [HttpGet("hello")]
    public async Task<IActionResult> GetHello([FromQuery] string name)
    {
        try
        {
            Console.WriteLine($"{DateTime.Now} | GET /hello called with parameter: {name}");
            var returnString = await gService.ReturnHello(name);
            var response = new Model() {Response = returnString};
            Console.WriteLine($"{DateTime.Now} | GET /hello response: {response.Response}");
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now} | Error occurred inside GET /hello controller, returning HTTP Status 500 with error message.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Same exact method, but this will call a fake greeting service that always errors to test errors.
    [HttpGet("sadflow")]
    public async Task<IActionResult> GetHelloSadFlow([FromQuery] string name)
    {
        try
        {
            Console.WriteLine($"{DateTime.Now} | GET /hello called with parameter: {name}");
            var returnString = await fakeService.ReturnHelloError(name);
            var response = new Model() {Response = returnString};
            Console.WriteLine($"{DateTime.Now} | GET /hello response: {response.Response}");
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now} | Error occurred inside GET /hello controller, returning HTTP Status 500 with error message.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}