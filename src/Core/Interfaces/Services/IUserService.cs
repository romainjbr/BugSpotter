using Core.Dtos.Users;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<User?> FindUserAsync(UserLoginDto dto, CancellationToken token);
    Task<List<UserReadDto>> ListAsync(CancellationToken token);
    Task<string?> RegisterAsync(UserCreateDto dto, CancellationToken token);
    Task<string?> LoginAsync(UserLoginDto dto, CancellationToken token);
    Task<bool> DeleteAsync(Guid id, CancellationToken token);
}