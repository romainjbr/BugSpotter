using Core.Entities;
using Core.Dtos.Sightings;
using Core.Dtos.Bugs;
using Core.Interfaces;
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

    private static Bug MakeBug() => new()
    {
        Id = Guid.NewGuid(),
        Species = "Wasp",
        DangerLevel = 3,
        Description = "Stingy"
    };
}
