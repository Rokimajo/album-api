using Album.Api.Services;
namespace Album.Api;
public class Program {
    public static void Main(string[] args) {
        Console.WriteLine($"{DateTime.Now} | Application is starting..");
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddTransient<IGreeting, GreetingService>();
        builder.Services.AddTransient<FakeGreetingService>();
        // Add services to the container.
        builder.Services.AddControllers();
        var app = builder.Build();
        app.MapControllers();
        app.Run();
        Console.WriteLine($"{DateTime.Now} | Application shut down.");
    }
}

