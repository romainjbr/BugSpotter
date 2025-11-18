using Core.Entities;
using Core.Dtos.Bugs;

namespace Core.Tests.Services.BugServiceTest;

public class BugServiceTests
{
    private static Bug MakeBug() => new()
    {
        Id = Guid.NewGuid(),
        Species = "Wasp",
        DangerLevel = 3,
        Description = "Stingy"
    };

    private static BugCreateDto MakeCreateDto() => new("Ant", 1, "Tiny");

    private static BugUpdateDto MakeUpdateDto(Guid id) => new(id, "Bee", 2, "Buzz");
}
