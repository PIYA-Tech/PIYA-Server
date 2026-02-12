namespace PIYA_API.Model;

public class Token
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreationTime { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

    public Token()
    {
        CreationTime = DateTime.UtcNow;
        ExpiresAt = CreationTime.AddMinutes(30);
    }
}
