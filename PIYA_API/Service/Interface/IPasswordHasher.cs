namespace PIYA_API.Service.Interface;

public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password
    /// </summary>
    string HashPassword(string password);
    
    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    bool VerifyPassword(string password, string hash);
}
