using System.Net;
using System.Net.Http.Json;
using Core.Dtos.Bugs;
using Core.Dtos.Users;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Tests.AuthenticationHelper;

namespace Presentations.Test.EndpointTests;

public class BugEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly  Mock<IBugService> _svc;

    private readonly HttpClient _client;

    public BugEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _svc = new Mock<IBugService>();
        _client = CreateClientWithService();
    }
    
    private HttpClient CreateClientWithService()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                 services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                services.AddSingleton<IBugService>(_svc.Object);
            });
        }).CreateClient();
    }

    #region GET /bug/all

    [Fact]
    public async Task GetAll_WhenServiceReturnsList_ReturnsOkWithList()
    {
        var bugs = new List<BugReadDto>
        {
            new(Guid.NewGuid(), "Wasp", 3, "Angry"),
            new(Guid.NewGuid(), "Ant", 1, "Tiny")
        };

        _svc.Setup(s => s.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bugs);

        var response = await _client.GetAsync("/bug/all");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<BugReadDto>>();
        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
        Assert.Contains(result, b => b.Species == "Wasp");
        Assert.Contains(result, b => b.Species == "Ant");

        _svc.Verify(s => s.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GET /bug/{id}

    [Fact]
    public async Task GetById_WhenServiceReturnsBug_ReturnsOkWithBug()
    {
        var bugId = Guid.NewGuid();
        var bugReadDto = new BugReadDto(bugId, "Bee", 2, "Buzzes");

        _svc.Setup(s => s.GetByIdAsync(bugId, It.IsAny<CancellationToken>())).ReturnsAsync(bugReadDto);

        var response = await _client.GetAsync($"/bug/{bugId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<BugReadDto>();
        Assert.NotNull(result);
        Assert.Equal(bugId, result!.Id);
        Assert.Equal("Bee", result.Species);

        _svc.Verify(s => s.GetByIdAsync(bugId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region POST /bug/create

    [Fact]
    public async Task Create_WhenServiceReturnsDto_ReturnsCreatedWithDto()
    {
        var newId = Guid.NewGuid();
        var bugCreateDto = new BugCreateDto("Spider", 2, "Creepy");
        var bugCreatedDto = new BugReadDto(newId, "Spider", 2, "Creepy");

        _svc.Setup(s => s.AddAsync(It.IsAny<BugCreateDto>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(bugCreatedDto);

        var response = await _client.PostAsJsonAsync("/bug/create", bugCreateDto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<BugReadDto>();
        Assert.NotNull(result);
        Assert.Equal(newId, result!.Id);
        Assert.Equal("Spider", result.Species);

        Assert.Contains(newId.ToString(), response.Headers.Location!.ToString());

        _svc.Verify(s => s.AddAsync(It.IsAny<BugCreateDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DELETE /bug/{id}

    [Fact]
    public async Task Delete_WhenCalled_CallsServiceAndReturnsOk()
    {
        var bugId = Guid.NewGuid();

        _svc.Setup(s => s.DeleteAsync(bugId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var response = await _client.DeleteAsync($"/bug/{bugId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Bug {bugId} deleted.", message);

        _svc.Verify(s => s.DeleteAsync(bugId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region PUT /bug/{id}

    [Fact]
    public async Task Update_WhenCalled_UsesRouteIdAndReturnsOk()
    {
        var routeId = Guid.NewGuid();
        var bodyId = Guid.NewGuid();
        
        var updateDto = new BugUpdateDto(bodyId, "Bee", 2, "Buzzes");

        _svc.Setup(s => s.UpdateAsync(It.IsAny<BugUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var response = await _client.PutAsJsonAsync($"/bug/{routeId}", updateDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var message = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Bug {routeId} updated.", message);

        _svc.Verify(s => s.UpdateAsync(
                It.Is<BugUpdateDto>(b =>
                    b.Id == routeId &&
                    b.Species == updateDto.Species &&
                    b.DangerLevel == updateDto.DangerLevel &&
                    b.Description == updateDto.Description),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}