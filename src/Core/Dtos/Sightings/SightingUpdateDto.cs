using System.Text.Json.Serialization;

namespace Core.Dtos.Sightings;

public record SightingUpdateDto(Guid Id, [property: JsonIgnore] Guid BugId, [property: JsonIgnore] Guid UserId, double Latitude, double Longitude, DateTime SeenAt, string Notes);
