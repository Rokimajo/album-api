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
            // fallback connection string here due to the task-definition.json not being available as environment variable outside ECS.
            // ECS will still use the correct environment variables inside task-definition.json.
            Console.WriteLine("Cannot read environment variable connection string, using fallback..");
            builder.Services.AddDbContext<AWSContext>(options =>
                options.UseNpgsql("Server=cnsd-db-209962794367.cip3pme240ba.us-east-1.rds.amazonaws.com;Port=5432;Database=albumdatabase;User Id=postgres;Password=rUAtb$Ri3L4puT*%"));
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