using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PIYA_API.Data;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class JwtService(PharmacyApiDbContext dbContext, IConfiguration configuration) : IJwtService
{
    private readonly PharmacyApiDbContext _dbContext = dbContext;
    private readonly IConfiguration _configuration = configuration;
    
    // Fallback values if not in configuration
    private const string DefaultSecretKey = "PIYA_SECRET_KEY_CHANGE_THIS_IN_PRODUCTION_MIN_32_CHARS";
    private const string DefaultIssuer = "PIYA_API";
    private const string DefaultAudience = "PIYA_Clients";
    private const int DefaultExpirationMinutes = 30;

    public Guid GetId(string token)
    {
        var tokenObj = _dbContext.Tokens.FirstOrDefault(t => t.AccessToken == token);
        return tokenObj == null ? Guid.Empty : tokenObj.Id;
    }

    public string? GenerateSecurityToken(string username)
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
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName)
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
        var tokenEntity = new Token
        {
            Id = Guid.NewGuid(),
            AccessToken = jwtToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            CreationTime = DateTime.UtcNow,
            DeviceInfo = "Web" // Can be enhanced to capture actual device info
        };

        _dbContext.Tokens.Add(tokenEntity);
        _dbContext.SaveChanges();

        return jwtToken;
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
