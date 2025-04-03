namespace PIYA_API.Model;

public class Token
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    private DateTime Expires { get; set; }
    public DateTime CreationTime { get; set; }
    public string DeviceInfo { get; set; }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= Expires;
    }

    public Token()
    {
        CreationTime = DateTime.UtcNow;
        Expires = CreationTime.AddMinutes(30);
    }
}
