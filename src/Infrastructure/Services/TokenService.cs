
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    readonly string _key;

    public TokenService(IConfiguration config)
    {
        _key = config["Key"]!;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };

        foreach (var claim in user.Roles)
        {
            claims.Add(new(ClaimTypes.Role, claim));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(

            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
            notBefore: DateTime.UtcNow,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}