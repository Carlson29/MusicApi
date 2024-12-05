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
        _data = new List<Artists_Songs>
        {
            new Artists_Songs { Id = 1, Artist_Id = 1, Song_Id = 1},
            new Artists_Songs { Id = 2, Artist_Id = 2, Song_Id = 2}
        };

        _mockDbset = new Mock<DbSet<Artists_Songs>>();
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.Provider).Returns(_data.AsQueryable().Provider);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.Expression).Returns(_data.AsQueryable().Expression);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.ElementType).Returns(_data.AsQueryable().ElementType);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.GetEnumerator()).Returns(_data.AsQueryable().GetEnumerator);
        };

        _mockDbset = new Mock<DbSet<Artists_Songs>>();
        _controller = new Artists_SongsController(_mockContext.Object);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.Expression).Returns(_data.AsQueryable().Expression);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.ElementType).Returns(_data.AsQueryable().ElementType);
        _mockDbset.As<IQueryable<Artists_Songs>>().Setup(m => m.GetEnumerator()).Returns(_data.AsQueryable().GetEnumerator);

        _context = new ArtistsContext(options);

        _controller = new Artists_SongsController(_mockContext.Object);

        _controller = new Artists_SongsController(_context);
    }

    private void SeedDatabase()
    {
        var result = await _controller.GetArtists_Songs();

        
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Artists_Songs>>>(result);
        var result = await _controller.GetArtists_Songs(1);

        var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
        var item = Assert.IsType<Artists_Songs>(actionResult.Value);
        Assert.Equal(1, item.Id);
    }

    [Fact]
    public async Task GetArtists_Songs_With_Valid_Id()
    {
        var result = await _controller.GetArtists_Songs(1);

        var actionResult = Assert.IsType<ActionResult<Artists_Songs>>(result);
        var item = Assert.IsType<Artists_Songs>(actionResult.Value);
        Assert.Equal(1, item.Id);


    public async Task PostArtists_Songs_ShouldAddItem()
    {
        var newArtistsSong = new Artists_Songs { Id = 3, Artist_Id = 3, Song_Id = 3 };
        _mockDbset.Setup(m => m.Add(It.IsAny<Artists_Songs>())).Callback<Artists_Songs>(_data.Add);
    {
        var result = await _controller.PostArtists_Songs(newArtistsSong);


        Assert.IsType<NotFoundResult>(result.Result);
    }

    public async Task PostArtists_Songs_ShouldAddItem()
    {
        var newArtistsSong = new Artists_Songs { Id = 3, Artist_Id = 3, Song_Id = 3 };
        _mockDbset.Setup(m => m.Add(It.IsAny<Artists_Songs>())).Callback<Artists_Songs>(_data.Add);

        var result = await _controller.PostArtists_Songs(newArtistsSong);
        var updatedArtistsSong = new Artists_Songs { Id = 1, Artist_Id = 10, Song_Id = 10 };
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _controller.PutArtists_Songs(1, updatedArtistsSong);
   
        var result = await _controller.PostArtists_Songs(duplicateItem);

        var result = await _controller.PostArtists_Songs(duplicateItem);


    [Fact]
    public async Task PutArtists_Songs_Id_Mismatch()
    {
        var updatedArtistsSong = new Artists_Songs { Id = 1, Artist_Id = 10, Song_Id = 10 };

        var result = await _controller.PutArtists_Songs(2, updatedArtistsSong);

        Assert.IsType<BadRequestObjectResult>(result);

    }
        var result = await _controller.PutArtists_Songs(1, updatedArtistsSong);
    public async Task DeleteArtists_Songs_andRemoveItem()
    {
        var artistSong = _data.First();
        _mockDbset.Setup(m => m.FindAsync(1)).ReturnsAsync(artistSong);
        _mockDbset.Setup(m => m.Remove(It.IsAny<Artists_Songs>())).Callback<Artists_Songs>(item => _data.Remove(item));

        var result = await _controller.DeleteArtists_Songs(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Single(_data);

    public async Task PostArtists_Songs_Duplicate()
    {
        var updatedArtistsSong = new Artists_Songs { Id = 1, Artist_Id = 10, Song_Id = 10 };

        var result = await _controller.PutArtists_Songs(2, updatedArtistsSong);

        Assert.IsType<BadRequestObjectResult>(result);

    }

    public async Task DeleteArtists_Songs_andRemoveItem()
    {
        var artistSong = _data.First();
        _mockDbset.Setup(m => m.FindAsync(1)).ReturnsAsync(artistSong);
        _mockDbset.Setup(m => m.Remove(It.IsAny<Artists_Songs>())).Callback<Artists_Songs>(item => _data.Remove(item));

        var result = await _controller.DeleteArtists_Songs(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Single(_data);

    }



}
