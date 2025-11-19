using Microsoft.Extensions.Logging;
using Core.Interfaces.Services;
using Core.Interfaces.Repositories;
using Core.Entities;
using Core.Dtos.Bugs;
using Core.Dtos.Sightings;

namespace Core.Services;

public class SightingService : ISightingService
{
    readonly ILogger<SightingService> _logger;
    readonly IRepository<Sighting> _repo;
    readonly IRepository<Bug> _bugRepo;

    public SightingService(ILogger<SightingService> logger, IRepository<Sighting> repository,
        IRepository<Bug> bugRepository)
    {
        _logger = logger;
        _repo = repository;
        _bugRepo = bugRepository;
    }

    public async Task<SightingReadDto?> AddAsync(SightingCreateDto dto, CancellationToken token)
    {
        _logger.LogInformation("Creation request for Sighting for Bug '{BudIg}'", dto.BugId);

        var bug = await _bugRepo.GetByIdAsync(dto.BugId, token);
        if (bug is null)
        {
            _logger.LogWarning("Cannot create Sighting: no Bug is link. Create a Bug first");
            return null;
        }        
        
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity, token);

        _logger.LogInformation("Sighting {SightingId} successfully created", entity.Id);

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        _logger.LogInformation("Delete request for Sighting {SightingId}", id);

        var entity = await _repo.GetByIdAsync(id, token);
        if (entity is null) 
        { 
            _logger.LogWarning("No Sighting with id {SightingId} found. Deletion unsuccessful", id);
            return false; 
        }

        await _repo.DeleteAsync(entity, token);

        _logger.LogInformation("Sighting with id {SightingId} successfully deleted", id);

        return true;
    }

    public async Task<SightingReadDto?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        return entity?.ToDto();
    }

    public async Task<List<SightingReadDto>> ListAsync(CancellationToken token)
    {
        var list = await _repo.ListAsync(token);
        return list.Select(x => x.ToDto()).ToList();
    }

    public async Task<bool> UpdateAsync(SightingUpdateDto dto, CancellationToken token)
    {
        _logger.LogInformation("Update request for Sighting {SightingId}", dto.Id);

        var entity = await _repo.GetByIdAsync(dto.Id, token);
        if (entity is null) 
        { 
            _logger.LogWarning("Cannot udpdate Sighting {SightingId}: not found", dto.Id);
            return false;     
        }

        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        entity.Notes = dto.Notes;
        entity.SeenAt = dto.SeenAt;

        await _repo.UpdateAsync(entity, token);

        _logger.LogInformation("Sighting {SightingId} updated successfully", dto.Id);
        return true;
    }
}