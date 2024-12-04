using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicApi.Controllers;
using MusicApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MusicApi.Tests
{
    public class Artists_SongsControllerTests
    {
        private DbContextOptions<ArtistsContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<ArtistsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task GetArtists_Songs_ReturnsAllItems()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 1, Song_Id = 1, Artist_Id = 1 });
                context.Artists_Songs.Add(new Artists_Songs { Id = 2, Song_Id = 2, Artist_Id = 2 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);

                // Act
                var result = await controller.GetArtists_Songs();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Artists_Songs>>>(result);
                var items = Assert.IsType<List<Artists_Songs>>(actionResult.Value);
                Assert.Equal(2, items.Count);
            }
        }

        [Fact]
        public async Task GetArtists_Songs_ById_ReturnsCorrectItem()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 1, Song_Id = 1, Artist_Id = 1 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);

                // Act
                var result = await controller.GetArtists_Songs(1);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
                var item = Assert.IsType<Artists_Songs>(actionResult.Value);
                Assert.Equal(1, item.Id);
            }
        }

        [Fact]
        public async Task GetArtists_Songs_ById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);

                // Act
                var result = await controller.GetArtists_Songs(999);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public async Task PostArtists_Songs_AddsItem()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);
                var newItem = new Artists_Songs { Id = 1, Song_Id = 1, Artist_Id = 1 };

                // Act
                var result = await controller.PostArtists_Songs(newItem);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
                var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
                var item = Assert.IsType<Artists_Songs>(createdResult.Value);

                Assert.Equal(1, item.Id);
                Assert.Equal(1, context.Artists_Songs.Count());
            }
        }

        [Fact]
        public async Task PutArtists_Songs_UpdatesItem()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 1, Song_Id = 1, Artist_Id = 1 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);
                var updatedItem = new Artists_Songs { Id = 1, Song_Id = 2, Artist_Id = 1 };

                // Act
                var result = await controller.PutArtists_Songs(1, updatedItem);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(2, context.Artists_Songs.Find(1).Song_Id);
            }
        }

        [Fact]
        public async Task PutArtists_Songs_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);
                var updatedItem = new Artists_Songs { Id = 1, Song_Id = 2, Artist_Id = 1 };

                // Act
                var result = await controller.PutArtists_Songs(2, updatedItem);

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteArtists_Songs_RemovesItem()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 1, Song_Id = 1, Artist_Id = 1 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);

                // Act
                var result = await controller.DeleteArtists_Songs(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(0, context.Artists_Songs.Count());
            }
        }

        [Fact]
        public async Task DeleteArtists_Songs_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);

                // Act
                var result = await controller.DeleteArtists_Songs(999);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
