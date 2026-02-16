namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for generating and validating time-limited QR codes
/// </summary>
public interface IQRService
{
    /// <summary>
    /// Generate a time-limited QR token (5-minute validity)
    /// </summary>
    string GenerateQrToken(Guid entityId, string entityType, int validityMinutes = 5);
    
    /// <summary>
    /// Validate QR token and return entity ID
    /// </summary>
    (bool IsValid, Guid EntityId, string EntityType) ValidateQrToken(string token);
    
    /// <summary>
    /// Generate HMAC-signed QR token
    /// </summary>
    string GenerateSignedQrToken(Guid entityId, string entityType, int validityMinutes = 5);
    
    /// <summary>
    /// Validate HMAC signature of QR token
    /// </summary>
    (bool IsValid, Guid EntityId, string EntityType, DateTime ExpiresAt) ValidateSignedQrToken(string signedToken);
    
    /// <summary>
    /// Generate QR code image as base64 string
    /// </summary>
    Task<string> GenerateQrCodeImageAsync(string data);
    
    /// <summary>
    /// Check if token is expired
    /// </summary>
    bool IsTokenExpired(DateTime expiresAt);
    
    /// <summary>
    /// Revoke/blacklist a token (for one-time use enforcement)
    /// </summary>
    Task RevokeTokenAsync(string token);
    
    /// <summary>
    /// Check if token has been revoked
    /// </summary>
    Task<bool> IsTokenRevokedAsync(string token);
}
