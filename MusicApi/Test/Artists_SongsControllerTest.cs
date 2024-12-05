
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class Artists_SongsControllerTests
{
    private readonly Artists_SongsController _controller;
    private readonly ArtistsContext _context;

    public Artists_SongsControllerTests()
    {

        var options = new DbContextOptionsBuilder<ArtistsContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ArtistsContext(options);

        //My Database Seed
        SeedDatabase();

        _controller = new Artists_SongsController(_context);
    }

    private void SeedDatabase()
    {
        if (!_context.Artists_Songs.Any())
        {
            _context.Artists_Songs.AddRange(
                new Artists_Songs { Id = 1, Song_Id = 101, Artist_Id = 201 },
                new Artists_Songs { Id = 2, Song_Id = 102, Artist_Id = 202 }
            );
            _context.SaveChanges();
        }
    }



    [Fact]
    public async Task GetArtists_Songs_All_Items()
    {

        var result = await _controller.GetArtists_Songs();


        var actionResult = Assert.IsType<ActionResult<IEnumerable<Artists_Songs>>>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Artists_Songs>>(actionResult.Value);
        Assert.Equal(4, model.Count());
    }

    [Fact]
    public async Task GetArtists_Songs_With_Valid_Id()
    {

        int validId = 1;


        var result = await _controller.GetArtists_Songs(validId);


        var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
        var model = Assert.IsType<Artists_Songs>(actionResult.Value);
        Assert.Equal(validId, model.Id);
    }


    [Fact]
    public async Task GetArtists_Songs_With_Invalid_Id_()
    {

        var result = await _controller.GetArtists_Songs(99);


        Assert.IsType<NotFoundResult>(result.Result);
    }
    [Fact]
    public async Task PostArtists_Songs_Adds_Item()
    {

        var newItem = new Artists_Songs { Id = 5, Song_Id = 103, Artist_Id = 203 };


        var result = await _controller.PostArtists_Songs(newItem);


        var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var model = Assert.IsType<Artists_Songs>(createdAtActionResult.Value);
        Assert.Equal(newItem.Id, model.Id);
        Assert.Equal(3, _context.Artists_Songs.Count());
    }

    [Fact]
    public async Task PutArtists_Songs_Id_Mismatch()
    {

        var existingItem = _context.Artists_Songs.First();


        var result = await _controller.PutArtists_Songs(999, existingItem);


        Assert.IsType<BadRequestResult>(result);
    }
    [Fact]
    public async Task PostArtists_Songs_Duplicate()
    {

        var existingItem = _context.Artists_Songs.First();
        var duplicateItem = new Artists_Songs { Song_Id = existingItem.Song_Id, Artist_Id = existingItem.Artist_Id };


        var result = await _controller.PostArtists_Songs(duplicateItem);


        Assert.Equal(4, _context.Artists_Songs.Count());
    }



}

