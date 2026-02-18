namespace PIYA_API.Model;

/// <summary>
/// Email verification token for new user registration
/// </summary>
public class EmailVerificationToken
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Associated user
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Verification token (hashed in database)
    /// </summary>
    public required string TokenHash { get; set; }
    
    /// <summary>
    /// Email address being verified
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Token creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Token expiration timestamp (typically 24 hours)
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Whether the token has been used
    /// </summary>
    public bool IsUsed { get; set; } = false;
    
    /// <summary>
    /// When the token was used
    /// </summary>
    public DateTime? UsedAt { get; set; }
    
    /// <summary>
    /// IP address when token was created
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent when token was created
    /// </summary>
    public string? UserAgent { get; set; }
}
