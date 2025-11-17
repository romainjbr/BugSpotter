using System.Text.Json.Serialization;

namespace Core.Dtos.Sightings;

public record SightingCreateDto(Guid BugId, [property: JsonIgnore] Guid UserId, double Latitude, double Longitude, DateTime SeenAt, string Notes, DateTime CreatedAt);