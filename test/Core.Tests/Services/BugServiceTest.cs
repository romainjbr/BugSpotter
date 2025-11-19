using Core.Entities;
using Core.Dtos.Bugs;
using Core.Interfaces.Services;
using Core.Interfaces.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Services.BugServiceTest;

public class BugServiceTests
{
    readonly Mock<ILogger<BugService>> _logger;
    readonly Mock<IRepository<Bug>> _repo;
    readonly BugService _svc;
    private static Bug MakeBug() => new()
    {
        Id = Guid.NewGuid(),
        Species = "Wasp",
        DangerLevel = 3,
        Description = "Stingy"
    };

    private static BugCreateDto MakeCreateDto() => new("Ant", 1, "Tiny");

    private static BugUpdateDto MakeUpdateDto(Guid id) => new(id, "Bee", 2, "Buzz");

    public BugServiceTests()
    {
        _logger = new Mock<ILogger<BugService>>();
        _repo = new Mock<IRepository<Bug>>();
        _svc = new BugService(_logger.Object, _repo.Object);        
    }

#region AddAsync 

    [Fact]
    public async Task AddAsync_CallsRepo_And_ReturnsDto()
    {
        var dto = MakeCreateDto();

        _repo.Setup(x => x.AddAsync(It.IsAny<Bug>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _svc.AddAsync(dto, CancellationToken.None);

        _repo.Verify(x => x.AddAsync(
            It.Is<Bug>(b =>
                b.Species == "Ant" &&
                b.DangerLevel == 1 &&
                b.Description == "Tiny"),
            It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal("Ant", result.Species);
        Assert.Equal(1, result.DangerLevel);
        Assert.Equal("Tiny", result.Description);
    }

#endregion

#region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_IdNotFound_ReturnsNull()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bug?)null);

        var result = await _svc.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_IdFound_ReturnsBug()
    {
        var existing = MakeBug();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var result = await _svc.GetByIdAsync(existing.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(result?.Id, existing.Id);
        Assert.Equal(result?.Species, existing.Species);

        _repo.Verify(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

#endregion

#region ListAsync

    [Fact]
    public async Task ListAsync_ReturnsListOfBugs()
    {
        var list = new List<Bug> { MakeBug(), MakeBug() };

        _repo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var allBugs = await _svc.ListAsync(CancellationToken.None);

        Assert.Equal(2, allBugs.Count);
        Assert.True(allBugs.All(x => list.Any(b => b.Id == x.Id)));
    }
#endregion

#region DeleteAsync

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bug?)null);

        var ok = await _svc.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.DeleteAsync(It.IsAny<Bug>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Found_ReturnsTrue()
    {
        var existing = MakeBug();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _repo.Setup(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await _svc.DeleteAsync(existing.Id, CancellationToken.None);

        Assert.True(ok);
        _repo.Verify(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

#endregion
}
