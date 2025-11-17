namespace Core.Dtos.Bugs;

public record BugReadDto(Guid Id, string Species, int DangerLevel, string Description);