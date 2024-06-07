using Microsoft.EntityFrameworkCore.Query;

namespace Album.Api.PostgreSQL;

// public int Id { get; set; }
// public string Name { get; set; }
// public string Artist { get; set; }
// public string ImageUrl { get; set; }

public static class DbInitializer
    {
        public static void Initialize(AWSContext context)
        {
            context.Database.EnsureCreated();
            if (context.Albums.Any())
            {
                return;
            }

            var albumArr = new AlbumModel[]
            {
                new AlbumModel {Name="Test Album One",Artist="Test Artist One",ImageUrl=""},
                new AlbumModel {Name="Test Album Two",Artist="Test Artist Two",ImageUrl=""},
                new AlbumModel {Name="Test Album Three",Artist="Test Artist Three",ImageUrl=""},
                new AlbumModel {Name="Test Album Four",Artist="Test Artist Four",ImageUrl=""},
                new AlbumModel {Name="Test Album Five",Artist="Test Artist Five",ImageUrl=""},
                new AlbumModel {Name="Test Album Six",Artist="Test Artist Six",ImageUrl=""},
                new AlbumModel {Name="Test Album Seven",Artist="Test Artist Seven",ImageUrl=""},
            };
            foreach (var album in albumArr)
            {
                context.Albums.Add(album);
            }
            context.SaveChanges();
        }
    }