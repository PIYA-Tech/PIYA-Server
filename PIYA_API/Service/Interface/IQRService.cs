using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for generating and validating time-limited QR codes with anti-replay protection
/// </summary>
public interface IQRService
{
    /// <summary>
    /// Generate a time-limited QR token for prescription (5-minute validity)
    /// Returns the token string to be embedded in QR code
    /// </summary>
    Task<(string Token, Guid TokenId)> GeneratePrescriptionQrTokenAsync(Guid prescriptionId, Guid userId, string? ipAddress = null, string? userAgent = null);
    
    /// <summary>
    /// Validate prescription QR token and mark as used (one-time use)
    /// </summary>
    Task<(bool IsValid, Guid PrescriptionId, string ErrorMessage)> ValidateAndUsePrescriptionQrTokenAsync(string token, Guid pharmacistUserId, string? ipAddress = null, string? userAgent = null);
    
    /// <summary>
    /// Generate time-limited QR token for any entity type
    /// If validityMinutes is null, uses configured default from appsettings
    /// </summary>
    Task<(string Token, Guid TokenId)> GenerateQrTokenAsync(Guid entityId, string entityType, Guid userId, int? validityMinutes = null, string? ipAddress = null, string? userAgent = null);
    
    /// <summary>
    /// Validate QR token and return entity information (does not mark as used)
    /// </summary>
    Task<(bool IsValid, Guid EntityId, string EntityType, DateTime ExpiresAt, string ErrorMessage)> ValidateQrTokenAsync(string token);
    
    /// <summary>
    /// Mark a QR token as used (for one-time use enforcement)
    /// </summary>
    Task<bool> MarkTokenAsUsedAsync(string token, Guid usedByUserId, string? ipAddress = null, string? userAgent = null);
    
    /// <summary>
    /// Revoke/blacklist a token (for security or cancellation)
    /// </summary>
    Task<bool> RevokeTokenAsync(string token, Guid revokedByUserId, string reason);
    
    /// <summary>
    /// Check token status (Active/Used/Expired/Revoked)
    /// </summary>
    Task<(QRTokenStatus Status, DateTime? ExpiresAt)> GetTokenStatusAsync(string token);
    
    /// <summary>
    /// Get all QR tokens for an entity (audit trail)
    /// </summary>
    Task<List<QRToken>> GetTokenHistoryAsync(Guid entityId, string entityType);
    
    /// <summary>
    /// Get active QR token for an entity (if exists and not expired)
    /// </summary>
    Task<QRToken?> GetActiveTokenAsync(Guid entityId, string entityType);
    
    /// <summary>
    /// Clean up expired tokens (background task)
    /// </summary>
    Task<int> CleanupExpiredTokensAsync(int daysOld = 7);
    
    /// <summary>
    /// Generate QR code image as base64 string (requires QRCoder package)
    /// </summary>
    Task<string> GenerateQrCodeImageAsync(string data);
}
