using Core.Dtos.Bugs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Endpoints;

public static class BugEndpoints
{
    public static IEndpointRouteBuilder MapBugEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/bug");

        group.MapGet("/all", async Task<Ok<List<BugReadDto>>> (IBugService svc, CancellationToken token) =>
        {
            var allTodos = await svc.ListAsync(token);

            return TypedResults.Ok(allTodos);
        });


        group.MapGet("/{id}", async Task<Ok<BugReadDto>> (IBugService svc, Guid id, CancellationToken token) =>
        {
            var dto = await svc.GetByIdAsync(id, token);

            return TypedResults.Ok(dto);
        });

        group.MapPost("/create", async Task<Created<BugReadDto>> (IBugService svc, BugCreateDto dto, CancellationToken token) =>
        {
            var resultDto = await svc.AddAsync(dto, token);

            return TypedResults.Created($"{resultDto.Id}", resultDto);
        });

        group.MapDelete("/{id}", async Task<Ok<string>> (IBugService svc, Guid id, CancellationToken token) => 
        {
            var result = await svc.DeleteAsync(id, token);
            return TypedResults.Ok($"Bug {id} deleted.");
        });

        group.MapPut("/{id}", async Task<Ok<string>> (IBugService svc, BugUpdateDto dto, Guid id, CancellationToken token) =>
        {
            var bug = dto with { Id = id };
            var result = await svc.UpdateAsync(dto, token);
            return TypedResults.Ok($"Bug {id} updated.");
        });

        return group;
    }
}