using Core.Dtos.Users;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Services.UserServiceTest;

public class UserServiceTests
{
    private readonly Mock<ILogger<UserService>> _logger;
    private readonly Mock<IUserRepository> _repo;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IPasswordHasherService> _passwordHasher;
    private readonly UserService _svc;

    public UserServiceTests()
    {
        _logger = new Mock<ILogger<UserService>>();
        _repo = new Mock<IUserRepository>();    
        _tokenService = new Mock<ITokenService>();
        _passwordHasher = new Mock<IPasswordHasherService>();

        _svc = new UserService(_logger.Object, _repo.Object, _tokenService.Object, _passwordHasher.Object);
    }

    private static User MakeUser() => new()
    {
        Id = Guid.NewGuid(),
        UserName = "romain",
        Email = "romain@deutschland.com",
        HashedPassword = "hash"
    };

    private static UserLoginDto MakeLoginDto(string? username, string? email)
        => new(username, email, "password");

    #region FindUserAsync

    [Fact]
    public async Task FindUserAsync_EmptyInput_ReturnsNull()
    {
        var dto = MakeLoginDto("   ", "   ");

        var result = await _svc.FindUserAsync(dto, CancellationToken.None);

        Assert.Null(result);
        _repo.Verify(x => x.FindByUsernameOrEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task FindUserAsync_UsesUsername_WhenPresent()
    {
        var user = MakeUser();
        var dto = MakeLoginDto("  romain  ", null);

        _repo.Setup(x => x.FindByUsernameOrEmailAsync("romain", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _svc.FindUserAsync(dto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("romain", result!.UserName);

        _repo.Verify(x => x.FindByUsernameOrEmailAsync("romain", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindUserAsync_UsesEmail_WhenUsernameIsEmpty()
    {
        var user = MakeUser();
        var dto = MakeLoginDto("   ", "romain@deutschland.com ");

        _repo.Setup(x => x.FindByUsernameOrEmailAsync("romain@deutschland.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _svc.FindUserAsync(dto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("romain@deutschland.com", result!.Email);

        _repo.Verify(x => x.FindByUsernameOrEmailAsync("romain@deutschland.com", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindUserAsync_UserNotFound_ReturnsNull()
    {
        var dto = MakeLoginDto("bob", null);

        _repo.Setup(x => x.FindByUsernameOrEmailAsync("bob", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _svc.FindUserAsync(dto, CancellationToken.None);

        Assert.Null(result);
    }

    #endregion

    #region ListAsync

    [Fact]
    public async Task ListAsync_ReturnsDtos()
    {
        var list = new List<User> { MakeUser(), MakeUser() };

        _repo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var result = await _svc.ListAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Username == list[0].UserName);
    }

    #endregion
}
