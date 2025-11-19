using Microsoft.Extensions.Logging;
using Core.Interfaces.Services;
using Core.Interfaces.Repositories;
using Core.Entities;
using Core.Dtos.Bugs;
using Core.Dtos.Users;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _repo;

    public UserService(ILogger<UserService> logger, IUserRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        _logger.LogInformation("Deletion request for User with id '{UserId}'", id);
        throw new NotImplementedException();
    }

    public async Task<User?> FindUserAsync(UserLoginDto dto, CancellationToken token)
    {
        var input = dto.Username?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            input = dto.Email?.Trim();
        }
        
        if (string.IsNullOrWhiteSpace(input))
        {
            _logger.LogWarning("No username and no email has been input. User cannot be found");
            return null;
        }

        var user = await _repo.FindByUsernameOrEmailAsync(input, token);

        return user;
    }
    
    public async Task<List<UserReadDto>> ListAsync(CancellationToken token)
    {
        var allUsers = await _repo.GetAllUser(token);
        return allUsers.Select(x => x.ToDto()).ToList();
    }

    /* TODO 
    - Implement PasswordHasherService to verify crendentials 
    - Implement TokenService to create a return a token when login is successful
    */ 
    public async Task<string?> LoginAsync(UserLoginDto dto, CancellationToken token)
    {
        var foundUser = await FindUserAsync(dto, token);

        if (foundUser is null)
        {
            _logger.LogWarning("User cannot be found. Login failed.");
            return "";
        }

        return "mock_token";
    }

    /* TODO 
    - Implement TokenService to create a return a token when registration is successful
    */ 
    public async Task<string?> RegisterAsync(UserCreateDto dto, CancellationToken token)
    {
        return "mock_token";
    }
}