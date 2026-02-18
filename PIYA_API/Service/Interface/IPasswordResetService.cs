using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for password reset functionality
/// </summary>
public interface IPasswordResetService
{
    /// <summary>
    /// Generate password reset token and send email
    /// </summary>
    Task<PasswordResetToken> GenerateResetTokenAsync(string email, string ipAddress, string userAgent);
    
    /// <summary>
    /// Validate reset token
    /// </summary>
    Task<bool> ValidateResetTokenAsync(string token);
    
    /// <summary>
    /// Reset password using token
    /// </summary>
    Task<bool> ResetPasswordAsync(string token, string newPassword, string ipAddress);
    
    /// <summary>
    /// Revoke password reset token
    /// </summary>
    Task RevokeTokenAsync(string token);
    
    /// <summary>
    /// Revoke all pending reset tokens for user
    /// </summary>
    Task RevokeAllTokensForUserAsync(Guid userId);
}
