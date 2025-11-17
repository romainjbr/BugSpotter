using Core.Entities;

namespace Core.Dtos.Sightings;

public static class SightingMapper
{
    public static SightingReadDto ToDto(this Sighting sighting)
    {
        return new SightingReadDto(sighting.Id, sighting.BugId, sighting.UserId, sighting.Latitude, sighting.Longitude, sighting.SeenAt, sighting.Notes);
    }

    public static Sighting ToEntity(this SightingCreateDto dto)
    {
        return new Sighting
        {
            Id = Guid.NewGuid(),
            BugId = dto.BugId,
            UserId = dto.UserId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            SeenAt = dto.SeenAt,
            Notes = dto.Notes,
            CreatedAt = dto.CreatedAt
        };
    }    
}