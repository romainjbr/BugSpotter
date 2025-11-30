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
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasherService _passwordHasher;

    public UserService(ILogger<UserService> logger, IUserRepository repo, ITokenService tokenService,
        IPasswordHasherService passwordHasher)
    {
        _logger = logger;
        _repo = repo;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
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

    public async Task<string?> LoginAsync(UserLoginDto dto, CancellationToken token)
    {
        var foundUser = await FindUserAsync(dto, token);

        if (foundUser is null || !_passwordHasher.VerifyCredentials(dto.Password, foundUser.HashedPassword)) 
        {
            _logger.LogWarning("User or password incorrect. Login failed.");
            return "";
        }

        _logger.LogWarning("Login for user {UserId} successfull", foundUser.Id);
        return _tokenService.CreateToken(foundUser);
    }

    public async Task<string?> RegisterAsync(UserCreateDto dto, CancellationToken token)
    {      
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        var user = dto.ToEntity();
        user.HashedPassword = hashedPassword;

        await _repo.AddUserAsync(user, token);

        _logger.LogWarning("Register for user {UserId} successfull", user.Id);
        return _tokenService.CreateToken(user);
    }
}