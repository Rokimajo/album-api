namespace Album.Api.Services;

// Greeting service that only throws exceptions to test sad flow.
public class FakeGreetingService
{
    public async Task<string> ReturnHelloError(string name)
    {
        Console.WriteLine($"{DateTime.Now} | ReturnHelloError() called. Returning mock exception.");
        throw new Exception("Test Exception for Sad Flow.");
    }
}