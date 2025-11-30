using System.Net;
using System.Net.Http.Json;
using Core.Dtos.Sightings;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Tests.AuthenticationHelper;

namespace Presentations.Test.EndpointTests;

public class SightingEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ISightingService> _svc;
    private readonly HttpClient _client; 

    public SightingEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _svc = new Mock<ISightingService>();
        _client = CreateClientWithService();
    }

    private HttpClient CreateClientWithService()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddSingleton<ISightingService>(_svc.Object);
            });
        }).CreateClient();
    }

    #region GET /sighting/all

    [Fact]
    public async Task GetAll_WhenServiceReturnsList_ReturnsOkWithList()
    {
        var list = new List<SightingReadDto>
        {
            new(
                Id: Guid.NewGuid(),
                BugId: Guid.NewGuid(),
                UserId: Guid.NewGuid(),
                Latitude: 48.0,
                Longitude: 2.0,
                SeenAt: new DateTime(2025, 1, 1, 12, 0, 0),
                Notes: "First"),
            new(
                Id: Guid.NewGuid(),
                BugId: Guid.NewGuid(),
                UserId: Guid.NewGuid(),
                Latitude: 49.0,
                Longitude: 3.0,
                SeenAt: new DateTime(2025, 1, 2, 13, 0, 0),
                Notes: "Second")
        };

        _svc.Setup(s => s.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var response = await _client.GetAsync("/sighting/all");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<SightingReadDto>>();
        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
        Assert.Contains(result, s => s.Notes == "First");
        Assert.Contains(result, s => s.Notes == "Second");

        _svc.Verify(s => s.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GET /sighting/{id}

    [Fact]
    public async Task GetById_WhenServiceReturnsSighting_ReturnsOkWithDto()
    {
        var id = Guid.NewGuid();
        var dto = new SightingReadDto(
            Id: id,
            BugId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            Latitude: 51.0,
            Longitude: 7.0,
            SeenAt: new DateTime(2025, 2, 1, 10, 0, 0),
            Notes: "Test sighting");

        _svc.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var response = await _client.GetAsync($"/sighting/{id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SightingReadDto>();

        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
        Assert.Equal("Test sighting", result.Notes);

        _svc.Verify(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DELETE /sighting/{id}

    [Fact]
    public async Task Delete_WhenCalled_CallsServiceAndReturnsOk()
    {
        var id = Guid.NewGuid();

        _svc.Setup(s => s.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var response = await _client.DeleteAsync($"/sighting/{id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("Sighting deleted.", message);

        _svc.Verify(s => s.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region PUT /sighting/{id}

    [Fact]
    public async Task Update_WhenCalled_UsesRouteIdAndReturnsOk()
    {
        var routeId = Guid.NewGuid();
        var bodyId = Guid.NewGuid();

        var updateDtoFromBody = new SightingUpdateDto(
            Id: bodyId,
            UserId: Guid.NewGuid(),
            BugId: Guid.NewGuid(), 
            Latitude: 42.0,
            Longitude: 7.0,
            SeenAt: new DateTime(2025, 3, 1, 15, 30, 0),
            Notes: "New note");

        _svc.Setup(s => s.UpdateAsync(It.IsAny<SightingUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var response = await _client.PutAsJsonAsync($"/sighting/{routeId}", updateDtoFromBody);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains("Sighting updated.", message);

        _svc.Verify(s => s.UpdateAsync(
                It.Is<SightingUpdateDto>(s =>
                    s.Id == routeId &&
                    s.Latitude == updateDtoFromBody.Latitude &&
                    s.Longitude == updateDtoFromBody.Longitude &&
                    s.SeenAt == updateDtoFromBody.SeenAt &&
                    s.Notes == updateDtoFromBody.Notes),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
