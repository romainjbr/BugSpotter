using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests.Services;

public class TokenServiceTests
{
    readonly TokenService _svc;
    readonly string _key = "example_of_secret_key_tokenservice_tests";
    public TokenServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Key"] = _key
            })
            .Build();

        _svc = new TokenService(config);        
    }

    private User MakeUser() => new()
    {
        Id = Guid.NewGuid(),
        Email = "romain@deutschland.com",
        UserName = "romain12",
        Roles = ["Admin", "User"]
    };

    [Fact]
    public void CreateToken_ReturnsValidJwtString()
    {
        var user = MakeUser();

        var token = _svc.CreateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(token));

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal(user.Id.ToString(), jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal(user.Email, jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        Assert.Equal(user.UserName, jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value);

        var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        Assert.Contains("Admin", roles);
        Assert.Contains("User", roles);
        Assert.True(jwt.ValidTo > DateTime.UtcNow);
    }
}
