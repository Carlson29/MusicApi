
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
using Xunit;

public class SongsControllerTests
{
    private readonly SongsController _controller;
    private readonly ArtistsContext _context;

    public SongsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ArtistsContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

        _context = new ArtistsContext(options);
        _controller = new SongsController(_context);


        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
    [Fact]
    public async Task GetSongs_ReturnsListOfSongs()
    {
        var songList = new List<Songs>
        {
            new Songs { Id = 4, Title = "Song 1", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now },
            new Songs { Id = 5, Title = "Song 2", Genre = "Rock", Duration = TimeSpan.FromMinutes(4), ReleaseDate = DateTime.Now }
        };

        await _context.Songs.AddRangeAsync(songList);
        await _context.SaveChangesAsync();

        var result = await _controller.GetSongs();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<SongsDto>>>(result);
        var model = Assert.IsAssignableFrom<List<SongsDto>>(actionResult.Value);
        Assert.Equal(4, model.Count);
    }

    [Fact]
    public async Task GetSongsById()
    {

        var songId = 1000;

        var result = await _controller.GetSongs(songId);


        var actionResult = Assert.IsType<ActionResult<SongsDto>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetSongsById_IfSongExists()
    {
        var songId = 3;
        var song = new Songs { Id = songId, Title = "Song 1", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now };
        await _context.Songs.AddAsync(song);
        await _context.SaveChangesAsync();

        var result = await _controller.GetSongs(songId);

        var actionResult = Assert.IsType<ActionResult<SongsDto>>(result);
        var model = Assert.IsType<SongsDto>(actionResult.Value);
        Assert.Equal("Song 1", model.Title);
        Assert.Equal(TimeSpan.FromMinutes(3), model.Duration);
    }

    [Fact]
    public async Task PostSongs_CreatedAtAction()
    {
        var song = new Songs { Id = 4, Title = "New Song", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now };

        var result = await _controller.PostSongs(song);

        var actionResult = Assert.IsType<ActionResult<Songs>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal("GetSongs", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task PutSongs_IfIdMismatch()
    {
        var songId = 1;
        var song = new Songs { Id = 2, Title = "Updated Song", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now };

        var result = await _controller.PutSongs(songId, song);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PutSongs_IfSuccessful()
    {
        var songId = 3;
        var song = new Songs { Id = songId, Title = "Updated Song", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now };
        await _context.Songs.AddAsync(song);
        await _context.SaveChangesAsync();

        var result = await _controller.PutSongs(songId, song);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSongs_IfSongDoesNotExist()
    {
        var songId = 5;

        var result = await _controller.DeleteSongs(songId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteSongs_IfSongDeleted()
    {
        var songId = 5;
        var song = new Songs { Id = songId, Title = "Song 1", Genre = "Pop", Duration = TimeSpan.FromMinutes(3), ReleaseDate = DateTime.Now };
        await _context.Songs.AddAsync(song);
        await _context.SaveChangesAsync();

        var result = await _controller.DeleteSongs(songId);

        Assert.IsType<NoContentResult>(result);
    }
}

