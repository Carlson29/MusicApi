using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicApi.Controllers;
using MusicApi.Models;
using MusicApi.Service;
using Xunit;

namespace MusicApi.Tests
{
    public class UsersControllerTests
    {
        private readonly DbContextOptions<ArtistsContext> _dbContextOptions;
        private readonly Mock<JwtService> _jwtServiceMock;
        private readonly ArtistsContext _context;
        private readonly UsersController _controller;



        public UsersControllerTests()
        {
            // Setup in-memory database options
            _dbContextOptions = new DbContextOptionsBuilder<ArtistsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create the in-memory context
            _context = new ArtistsContext(_dbContextOptions);

            // Mock JwtService (specific methods)
            _jwtServiceMock = new Mock<JwtService>(MockBehavior.Strict, null);
            _controller = new UsersController(_context, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithResponse()
        {
            // Arrange
            var request = new UserDto
            {
                User_Name = "validUser",
                Password = "validPassword"
            };

            var responseModel = new LoginResponseModel
            {
                UserName = "validUser",
                AccessToken = "someAccessToken",
                ExpiresIn = 3600
            };

            _jwtServiceMock
                .Setup(x => x.Authenticate(request))
                .ReturnsAsync(responseModel);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LoginResponseModel>>(result);
            var okResult = Assert.IsType<LoginResponseModel>(actionResult.Value);
            Assert.Equal(responseModel.UserName, okResult.UserName);
            Assert.Equal(responseModel.AccessToken, okResult.AccessToken);
            Assert.Equal(responseModel.ExpiresIn, okResult.ExpiresIn);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new UserDto
            {
                User_Name = "invalidUser",
                Password = "invalidPassword"
            };

            _jwtServiceMock
                .Setup(x => x.Authenticate(request))
                .ReturnsAsync((LoginResponseModel)null);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LoginResponseModel>>(result);
            Assert.IsType<UnauthorizedResult>(actionResult.Result);
        }
    }
}
