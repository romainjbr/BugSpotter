namespace Core.Dtos.Bugs;

public record BugCreateDto(string Species, int DangerLevel, string Description);