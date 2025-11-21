using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Tests.Services;

public class TokenServiceTests
{
    readonly TokenService _svc;
    
    public TokenServiceTests()
    {
        var jwtOptions = Options.Create(new JwtOptions
        {
           Key = "example_of_secret_key_tokenservice_tests",
           ExpirationMinutes = 60 
        });

        _svc = new TokenService(jwtOptions);        
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
