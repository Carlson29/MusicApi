using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MusicApi.Models
{
    public class ArtistsContext:DbContext
    {
        
            public DbSet<Artists> Artists { get; set; } = null;
           public DbSet<Songs> Songs { get; set; } = null;
        public DbSet<Artists_Songs> Artists_Songs { get; set; } = null;
        public DbSet<Users> Users { get; set; } = null;
        public string DbPath { get; }

            public ArtistsContext(DbContextOptions<ArtistsContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Artists>().HasData(
                new Artists()
                {
                    Id = 1,
                    Artist_Name = "Xjj",
                    Bio = "living life",
                    DateOfBirth = new DateTime(2000, 7, 22)

                },
                new Artists()
                {
                    Id = 2,
                    Artist_Name = "Paul",
                    Bio = "Paulina",
                    DateOfBirth = new DateTime(2000, 7, 22)

                });
             modelBuilder.Entity<Songs>().HasData(
                new Songs()
                {
                    Id= 1,
                    Title = "carl",
                    Genre ="Rap",
                    Duration = new TimeSpan(0, 2,45),
                    ReleaseDate = new DateTime(2008, 7, 23)

                },
                 new Songs()
                 {
                     Id = 2,
                     Title = "Punchy",
                     Genre = "Rap",
                     Duration = new TimeSpan(0, 2, 0),
                     ReleaseDate = new DateTime(2008, 8, 23)

                 }
                 );
         modelBuilder.Entity<Artists_Songs>().HasData(
                new Artists_Songs()
                {
                    Id = 1,
                    Song_Id = 1, 
                    Artist_Id= 1

                },
                new Artists_Songs()
                {
                    Id = 2,
                    Song_Id = 2,
                    Artist_Id = 2

                }

                 );
         modelBuilder.Entity<Users>().HasData(
                new Users()
                {
                    Id = 1,
                    User_Name = "carl",
                    Password = "14567Lp/"

                },
                new Users()
                {
                    Id = 2,
                    User_Name = "destiny",
                    Password = "14567Lp/"

                }
                 );
            base.OnModelCreating(modelBuilder);
            }

        }

    }

