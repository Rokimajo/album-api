namespace Album.Api.PostgreSQL;

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class AWSContext : DbContext
{
    public AWSContext(DbContextOptions<AWSContext> options) : base(options) {}
    public DbSet<AlbumModel> Albums { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}
}

public class AlbumModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string ImageUrl { get; set; }
}