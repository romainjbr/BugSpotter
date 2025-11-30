namespace Core.Dtos.Users;

public record UserReadDto(Guid Id, string Email, string Username, string HashedPassword, string[] Roles);