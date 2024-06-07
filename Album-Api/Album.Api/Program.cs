using Album.Api.PostgreSQL;
using Album.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Album.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"{DateTime.Now} | Application is starting..");
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddTransient<IGreeting, GreetingService>();
        builder.Services.AddTransient<IAlbumService, AlbumService>();
        builder.Services.AddTransient<FakeGreetingService>();
        var connectionString = Environment.GetEnvironmentVariable("AWSConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string is not set in environment variables.");
        }
        else
        {
            builder.Services.AddDbContext<AWSContext>(options =>
                options.UseNpgsql(connectionString));
        }
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        CreateDbIfNotExists(app);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();
        app.Run();
        Console.WriteLine($"{DateTime.Now} | Application shut down.");
    }

    private static void CreateDbIfNotExists(WebApplication host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AWSContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}