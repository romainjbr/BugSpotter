using Microsoft.Extensions.Logging;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos.Bugs;

namespace Core.Services;

public class BugService : IBugService
{
    public Task<BugReadDto> AddAsync(BugCreateDto dto, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<BugReadDto?> GetByIdAsync(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<List<BugReadDto>> ListAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(BugUpdateDto dto, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}