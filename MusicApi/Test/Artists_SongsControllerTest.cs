using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicApi.Controllers;
using MusicApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
public class Artists_SongsControllerTests
    {
    private readonly Mock<ArtistsContext> _mockContext;
    private readonly Mock<DbSet<Artists_Songs>> _mockDbset;
    private readonly Artists_SongsController _controller;
    private readonly List<Artists_Songs> _data;

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

        _mockContext = new Mock<ArtistsContext>();
    }
}

