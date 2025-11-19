using Core.Dtos.Bugs;

namespace Core.Interfaces.Services;

public interface IBugService
{
    Task<BugReadDto?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<BugReadDto>> ListAsync(CancellationToken token);
    Task<BugReadDto> AddAsync(BugCreateDto dto, CancellationToken token);
    Task<bool> UpdateAsync(BugUpdateDto dto, CancellationToken token);
    Task<bool> DeleteAsync(Guid id, CancellationToken token);
}