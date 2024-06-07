using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Album.Api.PostgreSQL;

public class AWSContextFactory : IDesignTimeDbContextFactory<AWSContext>
{
    public AWSContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AWSContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("AWSConnection"));

        return new AWSContext(optionsBuilder.Options);
    }
}