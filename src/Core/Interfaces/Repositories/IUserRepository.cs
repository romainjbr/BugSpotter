using Core.Dtos.Users;
using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByUsernameOrEmailAsync(string value, CancellationToken token);
}