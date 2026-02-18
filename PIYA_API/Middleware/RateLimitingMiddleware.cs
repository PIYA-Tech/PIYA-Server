using System.Collections.Concurrent;
using System.Net;

namespace PIYA_API.Middleware;

/// <summary>
/// Rate limiting middleware to prevent API abuse
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private static readonly ConcurrentDictionary<string, ClientRateLimitInfo> _clients = new();
    private readonly int _requestLimit;
    private readonly TimeSpan _timeWindow;
    private readonly List<string> _whitelistedPaths;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _requestLimit = int.Parse(configuration["RateLimit:RequestLimit"] ?? "100");
        _timeWindow = TimeSpan.FromMinutes(int.Parse(configuration["RateLimit:TimeWindowMinutes"] ?? "1"));
        _whitelistedPaths = configuration.GetSection("RateLimit:WhitelistedPaths").Get<List<string>>() ??
        [
            "/api/Health",
            "/swagger"
        ];

        // Cleanup task - runs every 5 minutes
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
                CleanupExpiredEntries();
            }
        });
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip rate limiting for whitelisted paths
        if (_whitelistedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await _next(context);
            return;
        }

        var clientId = GetClientIdentifier(context);
        var clientInfo = _clients.GetOrAdd(clientId, _ => new ClientRateLimitInfo());

        var now = DateTime.UtcNow;
        bool limitExceeded = false;
        int retryAfter = 0;
        int remaining = 0;
        long resetTime = 0;

        lock (clientInfo)
        {
            // Remove requests outside the time window
            clientInfo.Requests.RemoveAll(r => now - r > _timeWindow);

            // Check if limit exceeded
            if (clientInfo.Requests.Count >= _requestLimit)
            {
                limitExceeded = true;
                var oldestRequest = clientInfo.Requests.Min();
                retryAfter = (int)(_timeWindow - (now - oldestRequest)).TotalSeconds;
                resetTime = DateTimeOffset.UtcNow.AddSeconds(retryAfter).ToUnixTimeSeconds();
            }
            else
            {
                // Add current request
                clientInfo.Requests.Add(now);
                remaining = _requestLimit - clientInfo.Requests.Count;
                var nextReset = clientInfo.Requests.Min().Add(_timeWindow);
                resetTime = new DateTimeOffset(nextReset).ToUnixTimeSeconds();
            }
        }

        if (limitExceeded)
        {
            _logger.LogWarning("Rate limit exceeded for client {ClientId}. Path: {Path}", 
                clientId, context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.Headers["Retry-After"] = retryAfter.ToString();
            context.Response.Headers["X-RateLimit-Limit"] = _requestLimit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = "0";
            context.Response.Headers["X-RateLimit-Reset"] = resetTime.ToString();

            await context.Response.WriteAsJsonAsync(new
            {
                error = "Rate limit exceeded",
                message = $"Too many requests. Please try again in {retryAfter} seconds.",
                retryAfter
            });
            return;
        }

        // Add rate limit headers for successful requests
        context.Response.Headers["X-RateLimit-Limit"] = _requestLimit.ToString();
        context.Response.Headers["X-RateLimit-Remaining"] = remaining.ToString();
        context.Response.Headers["X-RateLimit-Reset"] = resetTime.ToString();

        await _next(context);
    }

    private static string GetClientIdentifier(HttpContext context)
    {
        // Try to get user ID from claims
        var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            return $"user:{userId}";
        }

        // Fall back to IP address
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        // Check for forwarded IP (when behind proxy)
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ip = context.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();
        }

        return $"ip:{ip}";
    }

    private static void CleanupExpiredEntries()
    {
        var keysToRemove = _clients
            .Where(kvp => !kvp.Value.Requests.Any())
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _clients.TryRemove(key, out _);
        }
    }
}

public class ClientRateLimitInfo
{
    public List<DateTime> Requests { get; } = new();
}
