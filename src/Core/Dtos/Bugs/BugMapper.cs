using Core.Entities;

namespace Core.Dtos.Bugs;

public static class BugMapper
{
    public static BugReadDto ToDto(this Bug bug)
    {
        return new BugReadDto(bug.Id, bug.Species, bug.DangerLevel, bug.Description);
    }

    public static Bug ToEntity(this BugCreateDto dto)
    {
        return new Bug
        {
            Id = Guid.NewGuid(),
            Species = dto.Species,
            DangerLevel = dto.DangerLevel,
            Description = dto.Description
        };
    }    
}