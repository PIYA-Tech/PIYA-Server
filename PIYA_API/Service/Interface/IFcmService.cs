namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for sending push notifications via Firebase Cloud Messaging
/// </summary>
public interface IFcmService
{
    /// <summary>
    /// Send a push notification to a specific device token
    /// </summary>
    Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null);
    
    /// <summary>
    /// Send a push notification to multiple device tokens
    /// </summary>
    Task<int> SendToMultipleAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string>? data = null);
    
    /// <summary>
    /// Send a push notification to a specific user (all their devices)
    /// </summary>
    Task<int> SendToUserAsync(Guid userId, string title, string body, Dictionary<string, string>? data = null);
    
    /// <summary>
    /// Register a device token for a user
    /// </summary>
    Task<bool> RegisterDeviceTokenAsync(Guid userId, string token, string platform, string? deviceModel = null, string? appVersion = null);
    
    /// <summary>
    /// Unregister a device token
    /// </summary>
    Task<bool> UnregisterDeviceTokenAsync(string token);
    
    /// <summary>
    /// Get all active device tokens for a user
    /// </summary>
    Task<List<string>> GetUserDeviceTokensAsync(Guid userId);
}
