using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for email verification functionality
/// </summary>
public interface IEmailVerificationService
{
    /// <summary>
    /// Generate and send email verification token
    /// </summary>
    Task<EmailVerificationToken> GenerateVerificationTokenAsync(Guid userId, string ipAddress, string userAgent);
    
    /// <summary>
    /// Verify email with token
    /// </summary>
    Task<bool> VerifyEmailAsync(string token);
    
    /// <summary>
    /// Resend verification email
    /// </summary>
    Task ResendVerificationEmailAsync(Guid userId);
    
    /// <summary>
    /// Check if email is already verified
    /// </summary>
    Task<bool> IsEmailVerifiedAsync(Guid userId);
    
    /// <summary>
    /// Revoke all pending verification tokens for user
    /// </summary>
    Task RevokeAllTokensAsync(Guid userId);
}
