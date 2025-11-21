namespace Presentation.Endpoints;

public static class BugEndpoints
{
    public static IEndpointRouteBuilder MapBugEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/bug");

        return group;
    }
}