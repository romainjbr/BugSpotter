using Core.Entities;

namespace Core.Interfaces.Services;

public interface ITokenService
{
    string CreateToken(User user);
}