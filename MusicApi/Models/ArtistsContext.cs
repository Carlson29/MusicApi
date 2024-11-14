using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MusicApi.Models
{
    public class ArtistsContext:DbContext
    {
        
            public DbSet<Artists> artists { get; set; } = null;
            public DbSet<Songs> songs { get; set; } = null;

            public string DbPath { get; }

            public ArtistsContext(DbContextOptions<ArtistsContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                /*modelBuilder.Entity<Movie>().HasData(
                    new Movie()
                    {
                        id = 1,
                        title = "boys in the hood",
                        description = "friends living life",
                        directorId = 1

                    });
                modelBuilder.Entity<Director>().HasData(
                   new Director()
                   {
                       id = 1,
                       name = "carl",
                       dateOfBirth = new DateTime(2000, 7, 23)

                   }
                    );*/
                base.OnModelCreating(modelBuilder);
            }

        }

    }
}
