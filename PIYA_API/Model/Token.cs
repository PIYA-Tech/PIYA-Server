namespace PIYA_API.Model;

public class Token
{
    public Guid Id { get; set; }
    public required string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    private DateTime Expires { get; set; }
    public required DateTime CreationTime { get; set; }
    public required string DeviceInfo { get; set; }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= Expires;
    }

    Token()
    {
        CreationTime = DateTime.UtcNow;
        Expires = CreationTime.AddMinutes(30);
    }
}
