using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using BCrypt
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password</returns>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) { throw new ArgumentException("Password cannot be null or empty", nameof(password)); }
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash)) { return false; }
        try { return BCrypt.Net.BCrypt.Verify(password, hash); }
        catch { return false; }
    }
}
