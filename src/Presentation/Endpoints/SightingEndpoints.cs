using System.Security.Claims;
using Core.Dtos.Sightings;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Endpoints;

public static class SightingEndpoints
{
    public static IEndpointRouteBuilder MapSightingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/sighting");

        group.MapGet("/all", async Task<Ok<List<SightingReadDto>>> (ISightingService svc, CancellationToken token) =>
        {
            var allTodos = await svc.ListAsync(token);

            return TypedResults.Ok(allTodos);
        });


        group.MapGet("/{id}", async Task<Ok<SightingReadDto>> (ISightingService svc, Guid id, CancellationToken token) =>
        {
            var dto = await svc.GetByIdAsync(id, token);

            return TypedResults.Ok(dto);
        });

        group.MapPost("/create", async Task<Created<SightingReadDto>> (ISightingService svc, HttpContext ctx, SightingCreateDto dto, CancellationToken token) =>
        {
            var id = Guid.Parse(ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createDto = dto with { UserId = id };

            var resultDto = await svc.AddAsync(createDto, token);

            return TypedResults.Created($"{resultDto?.Id}", resultDto);
        });

        group.MapDelete("/{id}", async Task<Ok<string>> (ISightingService svc, Guid id, CancellationToken token) => 
        {
            var result = await svc.DeleteAsync(id, token);
            return TypedResults.Ok("Sighting deleted.");
        });

        group.MapPut("/{id}", async Task<Ok<string>> (ISightingService svc, SightingUpdateDto dto, Guid id, CancellationToken token) =>
        {
            var bug = dto with { Id = id };
            var result = await svc.UpdateAsync(dto, token);
            return TypedResults.Ok("Sighting updated.");
        });

        return group;
    }
}