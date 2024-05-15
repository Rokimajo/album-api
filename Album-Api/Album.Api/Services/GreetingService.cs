namespace Album.Api.Services;

public class GreetingService : IGreeting
{
    public async Task<string> ReturnHello(string name)
    {
        // No error catching here to return error back to HelloController.cs
        string returnString = "";
        Console.WriteLine($"{DateTime.Now} | ReturnHello() called with " +
                          (String.IsNullOrWhiteSpace(name) ? "an empty or null name" : $"name: {name}"));
        if (String.IsNullOrWhiteSpace(name))
        {
            returnString = "Hello World";
        }
        else
        {
            returnString = $"Hello {name}";
        }

        Console.WriteLine($"{DateTime.Now} | ReturnHello() response: {returnString}");
        return returnString;
    }
}