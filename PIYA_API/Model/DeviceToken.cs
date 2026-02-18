namespace PIYA_API.Model;

/// <summary>
/// Device token for push notifications (FCM)
/// </summary>
public class DeviceToken
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// User who owns this device
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// FCM device registration token
    /// </summary>
    public required string Token { get; set; }
    
    /// <summary>
    /// Device platform (iOS, Android, Web)
    /// </summary>
    public required string Platform { get; set; }
    
    /// <summary>
    /// Device model/name (optional)
    /// </summary>
    public string? DeviceModel { get; set; }
    
    /// <summary>
    /// App version (optional)
    /// </summary>
    public string? AppVersion { get; set; }
    
    /// <summary>
    /// Whether the token is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// When the token was registered
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Last time the token was updated/verified
    /// </summary>
    public DateTime? LastUsedAt { get; set; }
}
