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
    public class UsersControllerTests
    {
        private readonly Mock<ArtistsContext> _mockContext;
        private readonly Mock<DbSet<Users>> _mockDbSet;
        private readonly List<Users> _usersData;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockContext = new Mock<ArtistsContext>();
            _mockDbSet = new Mock<DbSet<Users>>();
            _usersData = new List<Users>
            {
                new Users { Id = 1, Name = "User1" },
                new Users { Id = 2, Name = "User2" }
            };

            var queryable = _usersData.AsQueryable();

            _mockDbSet.As<IQueryable<Users>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockDbSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockDbSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockDbSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(_mockDbSet.Object);

            _controller = new UsersController(_mockContext.Object);
        }

        
        public async Task GetUsers_ReturnsAllUsers()
        {
          
            var result = await _controller.GetUsers();

          
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Users>>>(result);
            var returnValue = Assert.IsType<List<Users>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

       
        public async Task GetUsers_WithId()
        {
          
            _mockContext.Setup(c => c.Users.FindAsync(1)).ReturnsAsync(_usersData[0]);

          
            var result = await _controller.GetUsers(1);

            
            var actionResult = Assert.IsType<ActionResult<Users>>(result);
            var returnValue = Assert.IsType<Users>(actionResult.Value);
            Assert.Equal("User1", returnValue.Name);
        }

        
        public async Task GetUsers_WithInvalidId()
        {
          
            _mockContext.Setup(c => c.Users.FindAsync(It.IsAny<int>())).ReturnsAsync((Users)null);

          
            var result = await _controller.GetUsers(999);

            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        
        public async Task AddsNewUser()
        {
          
            var newUser = new Users { Id = 3, Name = "User3" };

            _mockContext.Setup(c => c.Users.Add(newUser));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.PostUsers(newUser);

            var actionResult = Assert.IsType<ActionResult<Users>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Users>(createdAtActionResult.Value);

            Assert.Equal("User3", returnValue.Name);
        }

       
        public async Task DeleteUsers_WithId()
        {
          
            _mockContext.Setup(c => c.Users.FindAsync(1)).ReturnsAsync(_usersData[0]);
            _mockContext.Setup(c => c.Users.Remove(It.IsAny<Users>()));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

           
            var result = await _controller.DeleteUsers(1);

          
            Assert.IsType<NoContentResult>(result);
        }

      
        public async Task DeleteUsers_WithInvalId()
        {
            
            _mockContext.Setup(c => c.Users.FindAsync(It.IsAny<int>())).ReturnsAsync((Users)null);

            
            var result = await _controller.DeleteUsers(999);

          
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
