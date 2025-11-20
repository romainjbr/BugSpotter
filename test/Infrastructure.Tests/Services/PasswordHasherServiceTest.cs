using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests.Services;

public class PasswordHasherServiceTest
{

    readonly PasswordHasherService _svc;

    public PasswordHasherServiceTest()
    {
        _svc = new PasswordHasherService();    
    }

    [Fact]
    public void VerifyHash_PasswordsNotMatching_ReturnFalse()
    {
        var savedPassword = "12345_pwd";
        var hashedPassword = _svc.HashPassword(savedPassword);

        var givenPassword = "123789_pwd";
        var isMatch = _svc.VerifyCredentials(givenPassword, hashedPassword);

        Assert.False(isMatch);
    }

    [Fact]
    public void VerifyHash_PasswordsMatching_ReturnTrue()
    {
        var savedPassword = "12345_pwd";
        var hashedPassword = _svc.HashPassword(savedPassword);

        var givenPassword = "12345_pwd";
        var isMatch = _svc.VerifyCredentials(givenPassword, hashedPassword);   

        Assert.True(isMatch);     
    }
}