using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Album.Api;
using Album.Api.PostgreSQL;
using Microsoft.Extensions.Configuration;

public class InMemoryApplicationFactory : WebApplicationFactory<Program>
{
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<AWSContext>)).ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AWSContext>(options =>
            { options.UseInMemoryDatabase("InMemoryDbForTesting");});

            var sp = services.BuildServiceProvider();
            
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AWSContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        });
    }
}