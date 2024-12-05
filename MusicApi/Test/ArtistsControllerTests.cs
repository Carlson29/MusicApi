using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
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
        
        var newArtist = new Artists
        {
            Artist_Name = "New Artist",
            Bio = "New Bio",
            DateOfBirth = DateTime.Parse("2000-01-01")
        };

       
        var result = await _controller.PostArtists(newArtist);

       
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdArtist = Assert.IsType<Artists>(createdResult.Value);
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
