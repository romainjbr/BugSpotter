using Core.Dtos.Users;
using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllUser(CancellationToken token);

    Task AddUserAsync(User user, CancellationToken token);

    Task DeleteUserAsync(User user, CancellationToken token);

    Task<User?> FindByUsernameOrEmailAsync(string value, CancellationToken token);
}