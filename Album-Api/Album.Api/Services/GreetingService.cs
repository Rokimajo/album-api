namespace Album.Api.Services;

public class GreetingService : IGreeting
{
    public async Task<string> ReturnHello(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("ReturnHello() called with an empty or null name.");
            return "Hello World";
        }

        Console.WriteLine($"ReturnHello() called with name: {name}.");
        return $"Hello {name}";
    }
}