using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicApi.Controllers;
using MusicApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

public class ArtistsControllerTests
{

    private readonly ArtistsContext _context;
    private readonly ArtistsController _controller;

    public ArtistsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ArtistsContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ArtistsContext(options);
        _controller = new ArtistsController(_context);

        SeedDatabase();
    }
    private void SeedDatabase()
    {
        if (!_context.Artists.Any())
        {
            _context.Artists.AddRange(
                new Artists { Id = 1, Artist_Name = "Artist1", Bio = "Bio1", DateOfBirth = DateTime.Parse("1990-01-01") },
                new Artists { Id = 2, Artist_Name = "Artist2", Bio = "Bio2", DateOfBirth = DateTime.Parse("1985-05-05") }
            );
            _context.SaveChanges();
        }
    }
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


                var result = await controller.GetArtists_Songs();


                var actionResult = Assert.IsType<ActionResult<IEnumerable<Artists_Songs>>>(result);
                var items = Assert.IsType<List<Artists_Songs>>(actionResult.Value);
                Assert.Equal(2, items.Count);
            }
        }

        [Fact]
        public async Task GetArtists_Songs_ById_ReturnsCorrectItem()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 7, Song_Id = 1, Artist_Id = 1 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);


                var result = await controller.GetArtists_Songs(1);

                var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
                var item = Assert.IsType<Artists_Songs>(actionResult.Value);
                Assert.Equal(1, item.Id);
            }
        }

        [Fact]
        public async Task GetArtists_Songs_ById_ReturnsNotFound_WhenItemDoesNotExist()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);


                var result = await controller.GetArtists_Songs(999);


                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public async Task PostArtists_Songs_AddsItem()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);
                var newItem = new Artists_Songs { Id = 9, Song_Id = 9, Artist_Id = 9 };


                var result = await controller.PostArtists_Songs(newItem);


                var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
                var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
                var item = Assert.IsType<Artists_Songs>(createdResult.Value);

                Assert.Equal(9, item.Id);
                Assert.Equal(5, context.Artists_Songs.Count());
            }
        }

        [Fact]
        public async Task PutArtists_Songs_UpdatesItem()
        {

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


                var result = await controller.PutArtists_Songs(1, updatedItem);


                Assert.IsType<NoContentResult>(result);
                Assert.Equal(2, context.Artists_Songs.Find(1).Song_Id);
            }
        }

        [Fact]
        public async Task PutArtists_Songs_ReturnsBadRequest_WhenIdMismatch()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);
                var updatedItem = new Artists_Songs { Id = 1, Song_Id = 2, Artist_Id = 1 };


                var result = await controller.PutArtists_Songs(2, updatedItem);


                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteArtists_Songs_RemovesItem()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                context.Artists_Songs.Add(new Artists_Songs { Id = 5, Song_Id = 1, Artist_Id = 1 });
                context.SaveChanges();
            }

            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);


                var result = await controller.DeleteArtists_Songs(1);

                Assert.IsType<NoContentResult>(result);
                Assert.Equal(2, context.Artists_Songs.Count());
            }
        }

        [Fact]
        public async Task DeleteArtists_Songs_ReturnsNotFound_WhenItemDoesNotExist()
        {

            var options = GetDbOptions();
            using (var context = new ArtistsContext(options))
            {
                var controller = new Artists_SongsController(context);


                var result = await controller.DeleteArtists_Songs(999);


                Assert.IsType<NotFoundResult>(result);
            }
        }
    }

    [Fact]
    public async Task GetArtists_ReturnsResults()
    {

        var result = await _controller.GetArtists();


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<ArtistsDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task GetArtists_ById_WhenFound()
    {

        var result = await _controller.GetArtists(1);


        var artistDto = Assert.IsType<ArtistsDto>(result.Value);
        Assert.Equal("Xjj", artistDto.Artist_Name);
    }

    [Fact]
    public async Task GetArtists_ById_WhenNotFound()
    {


        var result = await _controller.GetArtists(999);


        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostArtists()
    {
        var newArtist = new ArtistsDto
        {
            Artist_Name = "New Artist",
            Bio = "New Bio",
            DateOfBirth = DateTime.Parse("2000-01-01")
        };

        var result = await _controller.PostArtists(newArtist);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdArtist = Assert.IsType<ArtistsDto>(createdResult.Value);
        Assert.Equal("New Artist", createdArtist.Artist_Name);
    }


    [Fact]
    public async Task DeleteArtists_RemovesArtist()
    {

        var result = await _controller.DeleteArtists(1);


        Assert.IsType<NoContentResult>(result);


        Assert.Null(await _context.Artists.FindAsync(1));
    }

    [Fact]
    public async Task DeleteArtists_ReturnsNotFound_WhenArtistDoesNotExist()
    {

        var result = await _controller.DeleteArtists(999);


        Assert.IsType<NotFoundResult>(result);
    }
}