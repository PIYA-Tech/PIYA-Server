using Microsoft.AspNetCore.Mvc;
using PIYA_API.Model;
using PIYA_API.Service.Interface;

namespace PIYA_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, IJwtService jwtService, IConfiguration configuration) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                TokensInfo = new Token
                {
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty,
                    ExpiresAt = DateTime.UtcNow,
                    DeviceInfo = request.DeviceInfo ?? "Unknown"
                }
            };

            var createdUser = await _userService.Create(user, request.Password);
            
            var token = _jwtService.GenerateSecurityToken(createdUser.Username);

            if (token == null)
            {
                return StatusCode(500, new { message = "Failed to generate token" });
            }

            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 30;

            return Ok(new AuthResponse
            {
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userService.Authenticate(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = _jwtService.GenerateSecurityToken(user.Username);

            if (token == null)
            {
                return StatusCode(500, new { message = "Failed to generate token" });
            }

            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 30;

            return Ok(new AuthResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }

    [HttpPost("validate-token")]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
    {
        try
        {
            var username = _jwtService.ValidateToken(request.Token);

            if (username == null)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }

            return Ok(new
            {
                valid = true,
                username
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Token validation failed", error = ex.Message });
        }
    }
}

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public string? DeviceInfo { get; set; }
}

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class ValidateTokenRequest
{
    public required string Token { get; set; }
}

public class AuthResponse
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
