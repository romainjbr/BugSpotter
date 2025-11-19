using Core.Dtos.Sightings;

namespace Core.Interfaces.Services;

public interface ISightingService
{
    Task<SightingReadDto?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<SightingReadDto>> ListAsync(CancellationToken token);
    Task<SightingReadDto?> AddAsync(SightingCreateDto dto, CancellationToken token);
    Task<bool> UpdateAsync(SightingUpdateDto dto, CancellationToken token);
    Task<bool> DeleteAsync(Guid id, CancellationToken token);
}