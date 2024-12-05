using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MusicApi.Controllers;
using MusicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicApi.Tests
{
    public class SongsControllerTests
    {
        private readonly Mock<ArtistsContext> _mockContext;
        private readonly Mock<DbSet<Songs>> _mockDbSet;
        private readonly List<Songs> _songsData;
        private readonly SongsController _controller;

        public SongsControllerTests()
        {
            _mockContext = new Mock<ArtistsContext>();
            _mockDbSet = new Mock<DbSet<Songs>>();
            _songsData = new List<Songs>
            {
                new Songs { Id = 1, Title = "Song1", Artist = "Artist1" },
                new Songs { Id = 2, Title = "Song2", Artist = "Artist2" }
            };

            var queryable = _songsData.AsQueryable();

            _mockDbSet.As<IQueryable<Songs>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockDbSet.As<IQueryable<Songs>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockDbSet.As<IQueryable<Songs>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockDbSet.As<IQueryable<Songs>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext.Setup(c => c.Songs).Returns(_mockDbSet.Object);

            _controller = new SongsController(_mockContext.Object);
        }

      
      
        public async Task GetSongs_ReturnsAllSongs()
        {
            
            var result = await _controller.GetSongs();

            
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Songs>>>(result);
            var returnValue = Assert.IsType<List<Songs>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

       
        public async Task GetSongs_WithID()
        {
            
            _mockContext.Setup(c => c.Songs.FindAsync(1)).ReturnsAsync(_songsData[0]);

          
            var result = await _controller.GetSongs(1);

           
            var actionResult = Assert.IsType<ActionResult<Songs>>(result);
            var returnValue = Assert.IsType<Songs>(actionResult.Value);
            Assert.Equal("Song1", returnValue.Title);
        }

        
        public async Task GetSongs_WithoutID()
        {
           
            _mockContext.Setup(c => c.Songs.FindAsync(It.IsAny<int>())).ReturnsAsync((Songs)null);

          
            var result = await _controller.GetSongs(9999);

            
            Assert.IsType<NotFoundResult>(result.Result);
        }

       
        public async Task Add_Songs()
        {
            
            var newSong = new Songs { Id = 3, Title = "Song3", Artist = "Artist3" };

            _mockContext.Setup(c => c.Songs.Add(newSong));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

           
            var result = await _controller.PostSongs(newSong);

            
            var actionResult = Assert.IsType<ActionResult<Songs>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Songs>(createdAtActionResult.Value);

            Assert.Equal("Song3", returnValue.Title);
        }

      
        public async Task PutSongs_WithId()
        {
         
            var updatedSong = new Songs { Id = 1, Title = "UpdatedSong", Artist = "UpdatedArtist" };
            _mockContext.Setup(c => c.Entry(updatedSong).State = EntityState.Modified);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

          
            var result = await _controller.PutSongs(1, updatedSong);

          
            Assert.IsType<NoContentResult>(result);
        }

      
        public async Task PutSongs_ReturnsBadRequest()
        {
           
            var updatedSong = new Songs { Id = 1, Title = "UpdatedSong", Artist = "UpdatedArtist" };

            
            var result = await _controller.PutSongs(2, updatedSong);

         
            Assert.IsType<BadRequestResult>(result);
        }

       
        public async Task DeleteSongs_WithId()
        {
            
            _mockContext.Setup(c => c.Songs.FindAsync(1)).ReturnsAsync(_songsData[0]);
            _mockContext.Setup(c => c.Songs.Remove(It.IsAny<Songs>()));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

         
            var result = await _controller.DeleteSongs(1);

           
            Assert.IsType<NoContentResult>(result);
        }

        
        public async Task DeleteSongs_InvalidId()
        {
    
            _mockContext.Setup(c => c.Songs.FindAsync(It.IsAny<int>())).ReturnsAsync((Songs)null);

           
            var result = await _controller.DeleteSongs(999);

            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
