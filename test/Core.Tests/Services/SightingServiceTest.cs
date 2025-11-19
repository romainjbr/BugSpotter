using Core.Entities;
using Core.Dtos.Sightings;
using Core.Dtos.Bugs;
using Core.Interfaces.Services;
using Core.Interfaces.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Services.SightingServiceTest;

public class SightingServiceTests
{
    private readonly Mock<ILogger<SightingService>> _logger;
    private readonly Mock<IRepository<Sighting>> _repo;
    private readonly Mock<IRepository<Bug>> _bugRepo;
    private readonly SightingService _svc;

    private static Sighting MakeSighting() => new()
    {
        Id = Guid.NewGuid(),
        BugId = Guid.NewGuid(),
        UserId = Guid.NewGuid(),
        Latitude = 42.0,
        Longitude = 7.0,
        SeenAt = new DateTime(2025, 1, 1, 12, 0, 0),
        Notes = "Spotted near the tree",
        CreatedAt = new DateTime(2025, 1, 1, 12, 5, 0)
    };

    private static SightingCreateDto MakeCreateDto() =>
        new(
            BugId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            Latitude: 48.8566,
            Longitude: 2.3522,
            SeenAt: new DateTime(2025, 2, 1, 10, 0, 0),
            Notes: "On the balcony",
            CreatedAt: new DateTime(2025, 2, 1, 10, 0, 0)
        );

    private static SightingUpdateDto MakeUpdateDto(Guid id) =>
        new(
            Id: id,
            BugId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            Latitude: 50.0,
            Longitude: 8.0,
            SeenAt: new DateTime(2025, 3, 1, 15, 30, 0),
            Notes: "Moved closer to the window"
        );

    private static Bug MakeBug(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Species = "Wasp",
        DangerLevel = 3,
        Description = "Stingy"
    };

    public SightingServiceTests()
    {
        _logger = new Mock<ILogger<SightingService>>();
        _repo = new Mock<IRepository<Sighting>>();
        _bugRepo = new Mock<IRepository<Bug>>();
        _svc = new SightingService(_logger.Object, _repo.Object, _bugRepo.Object);
    }

    #region AddAsync

    [Fact]
    public async Task AddAsync_BugNotFound_ReturnsNull_AndDoesNotCallRepoAdd()
    {
        var dto = MakeCreateDto();

        _bugRepo.Setup(x => x.GetByIdAsync(dto.BugId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bug?)null);

        var result = await _svc.AddAsync(dto, CancellationToken.None);

        Assert.Null(result);
        _repo.Verify(x => x.AddAsync(It.IsAny<Sighting>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddAsync_BugFound_CallsRepoAndReturnsDto()
    {
        var dto = MakeCreateDto();
        var bug = MakeBug(dto.BugId);

        _bugRepo.Setup(x => x.GetByIdAsync(dto.BugId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bug);

        _repo.Setup(x => x.AddAsync(It.IsAny<Sighting>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _svc.AddAsync(dto, CancellationToken.None);

        _repo.Verify(x => x.AddAsync(
                It.Is<Sighting>(s =>
                    s.BugId == dto.BugId &&
                    s.UserId == dto.UserId &&
                    s.Latitude == dto.Latitude &&
                    s.Longitude == dto.Longitude &&
                    s.SeenAt == dto.SeenAt &&
                    s.Notes == dto.Notes),
                It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.NotNull(result);
        Assert.Equal(dto.BugId, result!.BugId);
        Assert.Equal(dto.UserId, result.UserId);
        Assert.Equal(dto.Latitude, result.Latitude);
        Assert.Equal(dto.Longitude, result.Longitude);
        Assert.Equal(dto.SeenAt, result.SeenAt);
        Assert.Equal(dto.Notes, result.Notes);
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_IdNotFound_ReturnsNull()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sighting?)null);

        var result = await _svc.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_IdFound_ReturnsSighting()
    {
        var existing = MakeSighting();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var result = await _svc.GetByIdAsync(existing.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(existing.Id, result!.Id);
        Assert.Equal(existing.BugId, result.BugId);
        Assert.Equal(existing.UserId, result.UserId);
        Assert.Equal(existing.Latitude, result.Latitude);
        Assert.Equal(existing.Longitude, result.Longitude);
        Assert.Equal(existing.SeenAt, result.SeenAt);
        Assert.Equal(existing.Notes, result.Notes);

        _repo.Verify(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region ListAsync

    [Fact]
    public async Task ListAsync_ReturnsListOfSightings()
    {
        var list = new List<Sighting> { MakeSighting(), MakeSighting() };

        _repo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var allSightings = await _svc.ListAsync(CancellationToken.None);

        Assert.Equal(2, allSightings.Count);
        Assert.All(allSightings, dto => Assert.Contains(list, s => s.Id == dto.Id));
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse_AndDoesNotDelete()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sighting?)null);

        var ok = await _svc.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.DeleteAsync(It.IsAny<Sighting>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Found_ReturnsTrue_AndDeletes()
    {
        var existing = MakeSighting();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _repo.Setup(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await _svc.DeleteAsync(existing.Id, CancellationToken.None);

        Assert.True(ok);
        _repo.Verify(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_NotFound_ReturnsFalse_AndDoesNotUpdate()
    {
        var dto = MakeUpdateDto(Guid.NewGuid());

        _repo.Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sighting?)null);

        var ok = await _svc.UpdateAsync(dto, CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.UpdateAsync(It.IsAny<Sighting>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Found_UpdatesEntityAndReturnsTrue()
    {
        var existing = MakeSighting();
        var dto = MakeUpdateDto(existing.Id);

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _repo.Setup(x => x.UpdateAsync(It.IsAny<Sighting>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await _svc.UpdateAsync(dto, CancellationToken.None);

        Assert.True(ok);

        _repo.Verify(x => x.UpdateAsync(
                It.Is<Sighting>(s =>
                    s.Id == dto.Id &&
                    s.Latitude == dto.Latitude &&
                    s.Longitude == dto.Longitude &&
                    s.SeenAt == dto.SeenAt &&
                    s.Notes == dto.Notes),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
