using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class JwtService(PharmacyApiDbContext dbContext, IConfiguration configuration) : IJwtService
{
    private readonly PharmacyApiDbContext _dbContext = dbContext;
    private readonly IConfiguration _configuration = configuration;
    
    private const string DefaultSecretKey = "PIYA_SECRET_KEY_CHANGE_THIS_IN_PRODUCTION_MIN_32_CHARS";
    private const string DefaultIssuer = "PIYA_API";
    private const string DefaultAudience = "PIYA_Clients";
    private const int DefaultExpirationMinutes = 30;

    public Guid GetId(string token)
    {
        var tokenObj = _dbContext.Tokens.FirstOrDefault(t => t.AccessToken == token);
        return tokenObj == null ? Guid.Empty : tokenObj.Id;
    }

    public TokenResponse? GenerateSecurityToken(string username)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return null;
        }

        // Get JWT settings from configuration
        var secretKey = _configuration["Jwt:SecretKey"] ?? DefaultSecretKey;
        var issuer = _configuration["Jwt:Issuer"] ?? DefaultIssuer;
        var audience = _configuration["Jwt:Audience"] ?? DefaultAudience;
        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) 
            ? mins 
            : DefaultExpirationMinutes;

        // Create security key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create claims
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("role", user.Role.ToString())
        };

        // Create token
        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        // Save token to database
        var refreshToken = GenerateRefreshToken();
        var tokenEntity = new Token
        {
            Id = Guid.NewGuid(),
            AccessToken = jwtToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            CreationTime = DateTime.UtcNow,
            DeviceInfo = "Web" // Can be enhanced to capture actual device info
        };

        _dbContext.Tokens.Add(tokenEntity);
        _dbContext.SaveChanges();

        return new TokenResponse
        {
            AccessToken = jwtToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<string?> RefreshAccessToken(string refreshToken)
    {
        var tokenEntity = await _dbContext.Tokens
            .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

        if (tokenEntity == null)
        {
            return null;
        }

        // Check if refresh token is expired (refresh tokens valid for 7 days)
        if (tokenEntity.CreationTime.AddDays(7) < DateTime.UtcNow)
        {
            return null;
        }

        // Find user by old token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenEntity.AccessToken);
        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(username))
        {
            return null;
        }

        // Generate new access token
        var newTokenResponse = GenerateSecurityToken(username);

        if (newTokenResponse == null)
        {
            return null;
        }

        // Update token entity with new access token
        tokenEntity.AccessToken = newTokenResponse.AccessToken;
        tokenEntity.ExpiresAt = newTokenResponse.ExpiresAt;

        await _dbContext.SaveChangesAsync();

        return newTokenResponse.AccessToken;
    }

    public string? ValidateToken(string token)
    {
        try
        {
            var secretKey = _configuration["Jwt:SecretKey"] ?? DefaultSecretKey;
            var issuer = _configuration["Jwt:Issuer"] ?? DefaultIssuer;
            var audience = _configuration["Jwt:Audience"] ?? DefaultAudience;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            
            // Extract username from claims
            var username = principal.FindFirst(ClaimTypes.Name)?.Value;
            
            return username;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new UnauthorizedAccessException("Token has expired");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException($"Token validation failed: {ex.Message}");
        }
    }
}
