using Core.Entities;

namespace Core.Dtos.Users;

public static class UserMapper
{
    public static UserReadDto ToDto(this User user)
    {
        return new UserReadDto(user.Id, user.Email, user.UserName, user.HashedPassword);
    }

    public static User ToEntity(this UserCreateDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.Username,
            Email = dto.Email,
            HashedPassword = dto.Password
        };
    }    
}