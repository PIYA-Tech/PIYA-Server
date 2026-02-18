namespace PIYA_API.Model;

/// <summary>
/// Password reset token for forgot password flow
/// </summary>
public class PasswordResetToken
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Associated user
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Reset token (hashed in database)
    /// </summary>
    public required string TokenHash { get; set; }
    
    /// <summary>
    /// Email address for password reset
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Token creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Token expiration timestamp (typically 1 hour)
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
    /// IP address when reset was requested
    /// </summary>
    public string? RequestIpAddress { get; set; }
    
    /// <summary>
    /// IP address when token was used
    /// </summary>
    public string? UsedIpAddress { get; set; }
    
    /// <summary>
    /// User agent when reset was requested
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// Whether the token was revoked (e.g., user requested new token)
    /// </summary>
    public bool IsRevoked { get; set; } = false;
    
    /// <summary>
    /// When the token was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }
}
