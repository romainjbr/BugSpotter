using Core.Dtos.Users;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        return routes;
    }
}