
using System.Security.Cryptography;
using System.Text;
using Core.Interfaces.Services;

namespace Infrastructure.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string passwordToHash)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(passwordToHash));
        return Convert.ToHexString(bytes);
    }

    public bool VerifyCredentials(string givenPassword, string actualPassword)
    {
        return actualPassword.Equals(HashPassword(givenPassword));
    }
}