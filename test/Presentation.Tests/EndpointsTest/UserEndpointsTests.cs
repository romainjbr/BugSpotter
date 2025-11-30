using System.Net;
using System.Net.Http.Json;
using Core.Dtos.Users;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Presentations.Test.EndpointTests;

public class UserEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly  Mock<IUserService> _svc;

    public UserEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _svc = new Mock<IUserService>();
    }

    [Fact]
    public async Task Register_WhenServiceReturnsEmptyString_ReturnsConflict()
    {
        _svc.Setup(s => s.RegisterAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync("");

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IUserService>(_svc.Object);
            });
        }).CreateClient();

        var dto = new UserCreateDto("romain", "romain@deutschlad.com", "passWord12!");

        var response = await client.PostAsJsonAsync("/user/register", dto);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Register_WhenServiceReturnsString_ReturnsOkString()
    {
        var mockToken = "f9ececad-9b32f2-13db67-a6fs-dsasda3456";

        _svc.Setup(s => s.RegisterAsync(It.IsAny<UserCreateDto>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(mockToken);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IUserService>(_svc.Object);
            });
        }).CreateClient();

        var dto = new UserCreateDto("romain", "romain@deutschlad.com", "passWord12!");

        var response = await client.PostAsJsonAsync("/user/register", dto);
        var value = await response.Content.ReadFromJsonAsync<string>();

        Assert.Equal(value, mockToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
