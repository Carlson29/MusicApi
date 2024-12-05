using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Models;
using Xunit;

namespace MusicApi.Tests
{
    public class ArtistsControllerTests
    {
        private Mock<DbSet<Artists>> GetMockDbSet(IEnumerable<Artists> data)
        {
            var queryableData = data.AsQueryable();

            var mockDbSet = new Mock<DbSet<Artists>>();
            mockDbSet.As<IQueryable<Artists>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<Artists>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<Artists>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<Artists>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            return mockDbSet;
        }

        private Mock<ArtistsContext> GetMockContext(Mock<DbSet<Artists>> mockDbSet)
        {
            var mockContext = new Mock<ArtistsContext>();
            mockContext.Setup(c => c.Artists).Returns(mockDbSet.Object);
            return mockContext;
        }

      
        public async Task GetArtists()
        {
            
            var mockArtists = new List<Artists>
            {
                new Artists { Id = 1, Name = "Artist1" },
                new Artists { Id = 2, Name = "Artist2" }
            };
            var mockDbSet = GetMockDbSet(mockArtists);
            var mockContext = GetMockContext(mockDbSet);

            var controller = new ArtistsController(mockContext.Object);

           
            var result = await controller.GetArtists();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Artists>>>(result);
            var returnValue = Assert.IsType<List<Artists>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

      
        public async Task GetArtists_InvalidId()
        {

            var mockArtists = new List<Artists>
            {
                new Artists { Id = 1, Name = "Artist1" }
            };
            var mockDbSet = GetMockDbSet(mockArtists);
            var mockContext = GetMockContext(mockDbSet);

            var controller = new ArtistsController(mockContext.Object);

            var result = await controller.GetArtists(2);

            Assert.IsType<NotFoundResult>(result.Result);
        }

      
        public async Task AddsNewArtist()
        {
            var mockArtists = new List<Artists>
            {
                new Artists { Id = 1, Name = "Artist1" }
            };
            var mockDbSet = GetMockDbSet(mockArtists);
            mockDbSet.Setup(m => m.Add(It.IsAny<Artists>())).Callback<Artists>(a => mockArtists.Add(a));
            var mockContext = GetMockContext(mockDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new ArtistsController(mockContext.Object);
            var newArtist = new Artists { Id = 2, Name = "Artist2" };

            var result = await controller.PostArtists(newArtist);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdArtist = Assert.IsType<Artists>(actionResult.Value);
            Assert.Equal("Artist2", createdArtist.Name);
            Assert.Equal(2, mockArtists.Count);
        }

       
        public async Task DeleteArtists()
        {

            var mockArtists = new List<Artists>
            {
                new Artists { Id = 1, Name = "Artist1" }
            };
            var mockDbSet = GetMockDbSet(mockArtists);
            mockDbSet.Setup(m => m.FindAsync(It.IsAny<int>())).ReturnsAsync((object[] ids) => mockArtists.FirstOrDefault(a => a.Id == (int)ids[0]));
            mockDbSet.Setup(m => m.Remove(It.IsAny<Artists>())).Callback<Artists>(a => mockArtists.Remove(a));
            var mockContext = GetMockContext(mockDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new ArtistsController(mockContext.Object);

            var result = await controller.DeleteArtists(1);


            Assert.IsType<NoContentResult>(result);
            Assert.Empty(mockArtists);
        }

       
        public async Task PutArtists()
        {
            var mockArtists = new List<Artists>
            {
                new Artists { Id = 1, Name = "Artist1" }
            };
            var mockDbSet = GetMockDbSet(mockArtists);
            var mockContext = GetMockContext(mockDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new ArtistsController(mockContext.Object);
            var updatedArtist = new Artists { Id = 1, Name = "UpdatedArtist" };

            var result = await controller.PutArtists(1, updatedArtist);


            Assert.IsType<NoContentResult>(result);
            Assert.Equal("UpdatedArtist", mockArtists.First().Name);
        }
    }
}
