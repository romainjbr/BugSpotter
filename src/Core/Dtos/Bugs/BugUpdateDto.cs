namespace Core.Dtos.Bugs;

public record BugUpdateDto(Guid Id, string Species, int DangerLevel, string Description);