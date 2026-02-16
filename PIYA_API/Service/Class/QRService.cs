using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class QRService : IQRService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<QRService> _logger;
    private readonly HashSet<string> _revokedTokens = new(); // In-memory revocation list
    private readonly string _secretKey;

    public QRService(IConfiguration configuration, ILogger<QRService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _secretKey = _configuration["Security:QrSigningKey"] ?? "DEFAULT_QR_SECRET_KEY_CHANGE_IN_PRODUCTION";
    }

    public string GenerateQrToken(Guid entityId, string entityType, int expiryMinutes = 5)
    {
        var payload = new
        {
            EntityId = entityId,
            EntityType = entityType,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            GeneratedAt = DateTime.UtcNow
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var bytes = Encoding.UTF8.GetBytes(jsonPayload);
        return Convert.ToBase64String(bytes);
    }

    public string GenerateSignedQrToken(Guid entityId, string entityType, int expiryMinutes = 5)
    {
        var payload = new
        {
            EntityId = entityId,
            EntityType = entityType,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            GeneratedAt = DateTime.UtcNow,
            Nonce = Guid.NewGuid().ToString() // Prevent duplicate tokens
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var signature = GenerateHmacSignature(jsonPayload);

        var signedPayload = new
        {
            Payload = jsonPayload,
            Signature = signature
        };

        var signedJson = JsonSerializer.Serialize(signedPayload);
        var bytes = Encoding.UTF8.GetBytes(signedJson);
        return Convert.ToBase64String(bytes);
    }

    public (bool IsValid, Guid EntityId, string EntityType) ValidateQrToken(string token)
    {
        try
        {
            var bytes = Convert.FromBase64String(token);
            var json = Encoding.UTF8.GetString(bytes);
            var payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (payload == null)
            {
                return (false, Guid.Empty, string.Empty);
            }

            var entityId = Guid.Parse(payload["EntityId"].GetString()!);
            var entityType = payload["EntityType"].GetString()!;

            return (true, entityId, entityType);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate QR token");
            return (false, Guid.Empty, string.Empty);
        }
    }

    public (bool IsValid, Guid EntityId, string EntityType, DateTime ExpiresAt) ValidateSignedQrToken(string token)
    {
        try
        {
            var bytes = Convert.FromBase64String(token);
            var json = Encoding.UTF8.GetString(bytes);
            var signedPayload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (signedPayload == null)
            {
                return (false, Guid.Empty, string.Empty, DateTime.MinValue);
            }

            var payloadJson = signedPayload["Payload"].GetString()!;
            var signature = signedPayload["Signature"].GetString()!;

            // Verify signature
            var expectedSignature = GenerateHmacSignature(payloadJson);
            if (signature != expectedSignature)
            {
                _logger.LogWarning("QR token signature verification failed");
                return (false, Guid.Empty, string.Empty, DateTime.MinValue);
            }

            // Parse payload
            var payload = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadJson);
            if (payload == null)
            {
                return (false, Guid.Empty, string.Empty, DateTime.MinValue);
            }

            var entityId = Guid.Parse(payload["EntityId"].GetString()!);
            var entityType = payload["EntityType"].GetString()!;
            var expiresAt = payload["ExpiresAt"].GetDateTime();

            return (true, entityId, entityType, expiresAt);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate signed QR token");
            return (false, Guid.Empty, string.Empty, DateTime.MinValue);
        }
    }

    public async Task<string> GenerateQrCodeImageAsync(string data)
    {
        // Placeholder - requires QRCoder NuGet package
        // Install: dotnet add package QRCoder
        // Implementation:
        // using QRCoder;
        // var qrGenerator = new QRCodeGenerator();
        // var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        // var qrCode = new PngByteQRCode(qrCodeData);
        // var bytes = qrCode.GetGraphic(20);
        // return Convert.ToBase64String(bytes);

        _logger.LogWarning("GenerateQrCodeImageAsync not implemented - requires QRCoder package");
        await Task.CompletedTask;
        return string.Empty;
    }

    public bool IsTokenExpired(DateTime expiresAt)
    {
        return DateTime.UtcNow > expiresAt;
    }

    public Task RevokeTokenAsync(string token)
    {
        _revokedTokens.Add(token);
        _logger.LogInformation("QR token revoked");
        return Task.CompletedTask;
    }

    public Task<bool> IsTokenRevokedAsync(string token)
    {
        return Task.FromResult(_revokedTokens.Contains(token));
    }

    private string GenerateHmacSignature(string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}
