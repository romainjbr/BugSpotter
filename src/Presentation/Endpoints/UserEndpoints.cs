using Core.Dtos.Users;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/user");

        group.MapPost("/register", async Task<Results<Ok<string>, Conflict<string>>> (IUserService svc, UserCreateDto dto, CancellationToken token) =>
        {
            var receivedToken = await svc.RegisterAsync(dto, token);

            if (string.IsNullOrWhiteSpace(receivedToken))
            {
                return TypedResults.Conflict("User already exists");
            }

            return TypedResults.Ok(receivedToken);
        });

        group.MapPost("/login", async Task<Results<Ok<string>, Conflict<string>>> (IUserService svc, UserLoginDto dto, CancellationToken token) =>
        {
            var receivedToken = await svc.LoginAsync(dto, token);

            if (string.IsNullOrWhiteSpace(receivedToken))
            {
                return TypedResults.Conflict("Email or password is incorrect");
            }

            return TypedResults.Ok(receivedToken);
        });

        // TODO: Implemet Authorization/Authentication and make this method accessible for authorized users only
        group.MapGet("/all", async Task<Results<Ok<List<UserReadDto>>, UnauthorizedHttpResult>> (IUserService svc, CancellationToken token) =>
        {
            var allUsers = await svc.ListAsync(token);

            return TypedResults.Ok(allUsers);
        }).RequireAuthorization();
        
       return routes;
    }
}