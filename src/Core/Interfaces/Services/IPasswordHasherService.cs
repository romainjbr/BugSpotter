namespace Core.Interfaces.Services;

public interface IPasswordHasherService
{
    string HashPassword(string passwordToHash);
    bool VerifyCredentials(string givenPassword, string actualPassword);
}