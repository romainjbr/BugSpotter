using Core.Dtos.Users;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly BugDb _db;
    public EfUserRepository(BugDb db)
    {
        _db = db;
    }

    public async Task AddUserAsync(User user, CancellationToken token)
    {
        await _db.Users.AddAsync(user, token);

        await _db.SaveChangesAsync(token);
    }

    public async Task DeleteUserAsync(User user, CancellationToken token)
    {
        _db.Users.Remove(user);

        await _db.SaveChangesAsync(token);
    }

    public async Task<User?> FindByUsernameOrEmailAsync(string value, CancellationToken token)
    {
        return await _db.Users.FirstOrDefaultAsync(
            u => u.UserName == value || u.Email == value,
            token);
    }

    public async Task<List<User>> GetAllUser(CancellationToken token)
    {
        return await _db.Users
         .AsNoTracking()
         .ToListAsync(token);
    }
}
