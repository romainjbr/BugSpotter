using Core.Dtos.Users;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfUserRepository : EfRepository<User>, IUserRepository
{
    public EfUserRepository(BugDb db): base(db) { }

    public async Task<User?> FindByUsernameOrEmailAsync(string value, CancellationToken token)
    {
        return await _db.Users.FirstOrDefaultAsync(
            u => u.UserName == value || u.Email == value,
            token);
    }
}
