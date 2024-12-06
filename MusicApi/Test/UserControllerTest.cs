using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicApi.Controllers;
using MusicApi.Models;
using MusicApi.Service;
using Xunit;

public class UsersControllerTests
{
    [Fact]
    public async Task Login_ValidCredentials()
    {
        // Arrange
        var mockJwtService = new Mock<JwtService>();
        var mockContext = new Mock<ArtistsContext>();
        var userDto = new UserDto { User_Name = "testuser", Password = "testpassword" };
        var expectedResponse = new LoginResponseModel
        {
            UserName = "testuser",
            AccessToken = "valid_access",
            ExpiresIn = 3600 
        };

        mockJwtService
            .Setup(service => service.Authenticate(It.Is<UserDto>(u => u.User_Name == userDto.User_Name && u.Password == userDto.Password)))
            .ReturnsAsync(expectedResponse);

        var controller = new UsersController(mockContext.Object, mockJwtService.Object);

        var result = await controller.Login(userDto);

        var actionResult = Assert.IsType<ActionResult<LoginResponseModel>>(result);
        var okResult = Assert.IsType<LoginResponseModel>(actionResult.Value);

        Assert.Equal(expectedResponse.UserName, okResult.UserName);
        Assert.Equal(expectedResponse.AccessToken, okResult.AccessToken);
        Assert.Equal(expectedResponse.ExpiresIn, okResult.ExpiresIn);

        mockJwtService.Verify(service => service.Authenticate(It.IsAny<UserDto>()), Times.Once);
    }

    [Fact]
    public async Task Login_InvalidCredentials()
    {
        var mockJwtService = new Mock<JwtService>();
        var mockContext = new Mock<ArtistsContext>();
        var userDto = new UserDto { User_Name = "wronguser", Password = "wrongpassword" };

        mockJwtService
            .Setup(service => service.Authenticate(It.IsAny<UserDto>()))
            .ReturnsAsync((LoginResponseModel)null);

        var controller = new UsersController(mockContext.Object, mockJwtService.Object);

        var result = await controller.Login(userDto);

        var actionResult = Assert.IsType<ActionResult<LoginResponseModel>>(result);
        Assert.IsType<UnauthorizedResult>(actionResult.Result);

        mockJwtService.Verify(service => service.Authenticate(It.IsAny<UserDto>()), Times.Once);
    }
}
