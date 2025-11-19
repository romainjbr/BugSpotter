using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Tests;

public class EfRepositoryTest
{
    private BugDb MakeDbContext()
    {
        var options = new DbContextOptionsBuilder<BugDb>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var db = new BugDb(options);
        db.Database.OpenConnection();
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task AddAndGetById_Works_WithRealDatabase()
    {
        using var db = MakeDbContext();
        var repo = new EfRepository<Bug>(db);

        var bug = new Bug
        {
            Id = Guid.NewGuid(),
            Species = "Ant",
            DangerLevel = 1,
            Description = "Tiny"
        };

        await repo.AddAsync(bug, CancellationToken.None);

        var result = await repo.GetByIdAsync(bug.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Ant", result!.Species);
    }
}
