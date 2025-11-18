using Microsoft.Extensions.Logging;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos.Bugs;

namespace Core.Services;

public class BugService : IBugService
{
    readonly ILogger<BugService> _logger;
    readonly IRepository<Bug> _repo;

    public BugService(ILogger<BugService> logger, IRepository<Bug> repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<BugReadDto> AddAsync(BugCreateDto dto, CancellationToken token)
    {
        var entity = dto.ToEntity();

        await _repo.AddAsync(entity, token);

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        if (entity is null) return false;

        await _repo.DeleteAsync(entity, token);
        return true;
    }

    public async Task<BugReadDto?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        return entity?.ToDto();
    }

    public async Task<List<BugReadDto>> ListAsync(CancellationToken token)
    {
        var bugList = await _repo.ListAsync(token);
        return bugList.Select(x => x.ToDto()).ToList();    
    }

    public async Task<bool> UpdateAsync(BugUpdateDto dto, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(dto.Id, token);
        if (entity is null) { return false; }

        entity.Species = dto.Species.Trim();
        entity.DangerLevel = dto.DangerLevel;
        entity.Description = dto.Description;

        await _repo.UpdateAsync(entity, token);
        return true;
    }
}