namespace PIYA_API.Model;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    
    /// <summary>
    /// Deprecated: Use PasswordHash instead
    /// </summary>
    [Obsolete("Use PasswordHash instead")]
    public string? Password { get; set; }
    
    /// <summary>
    /// BCrypt hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required Token TokensInfo { get; set; }
    public string? SigningKey { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}