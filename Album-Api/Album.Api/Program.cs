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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
        Console.WriteLine($"{DateTime.Now} | Application shut down.");
    }
}

