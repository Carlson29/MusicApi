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
            modelBuilder.Entity<Artists>().HasData(
                new Artists()
                {
                    Artist_Id = 1,
                    Artist_Name = "Xjj",
                    Bio = "living life",
                    DateOfBirth = new DateTime(2000, 7, 22)

                });
                modelBuilder.Entity<Songs>().HasData(
                   new Songs()
                   {
                       Song_Id= 1,
                       Title = "carl",
                       Genre ="Rap",
                       Duration = new TimeSpan(0, 2,45),
                       ReleaseDate = new DateTime(2000, 7, 23)

                   }
                    );
            modelBuilder.Entity<Artists_Songs>().HasData(
                   new Artists_Songs()
                   {
                       Id = 1,
                       Song_Id = 1, 
                       Artist_Id= 1

                   }
                    );
            modelBuilder.Entity<Users>().HasData(
                   new Users()
                   {
                       User_Id = 1,
                       Password = "14567Lp/"

                   }
                    );
            base.OnModelCreating(modelBuilder);
            }

        }

    }

