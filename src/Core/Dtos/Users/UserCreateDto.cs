namespace Core.Dtos.Users;

public record UserCreateDto(string Email, string Username, string Password, string[] Roles);