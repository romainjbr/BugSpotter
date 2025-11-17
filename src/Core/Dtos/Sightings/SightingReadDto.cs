namespace Core.Dtos.Sightings;

public record SightingReadDto(Guid Id, Guid BugId, Guid UserId, double Latitude, double Longitude, DateTime SeenAt, string Notes);
