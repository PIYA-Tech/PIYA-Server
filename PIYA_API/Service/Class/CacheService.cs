using Microsoft.Extensions.Caching.Distributed;
using PIYA_API.Service.Interface;
using System.Text.Json;

namespace PIYA_API.Service.Class;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _defaultExpiration;

    public CacheService(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
        _defaultExpiration = TimeSpan.FromMinutes(
            int.Parse(_configuration["Caching:DefaultExpirationMinutes"] ?? "60"));
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var cachedData = await _cache.GetStringAsync(key);
        
        if (string.IsNullOrEmpty(cachedData))
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        if (value == null)
        {
            return;
        }

        var serializedData = JsonSerializer.Serialize(value);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration
        };

        await _cache.SetStringAsync(key, serializedData, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var value = await _cache.GetStringAsync(key);
        return !string.IsNullOrEmpty(value);
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null) where T : class
    {
        // Try to get from cache first
        var cachedValue = await GetAsync<T>(key);
        
        if (cachedValue != null)
        {
            return cachedValue;
        }

        // If not in cache, get from factory
        var value = await factory();
        
        if (value != null)
        {
            await SetAsync(key, value, expiration);
        }

        return value;
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        // Note: This is a simplified implementation
        // For production, you would use Redis SCAN command with pattern matching
        // This requires direct Redis connection (not IDistributedCache)
        // For now, this is a placeholder that logs a warning
        
        Console.WriteLine($"Warning: RemoveByPatternAsync with pattern '{pattern}' is not fully implemented for distributed cache. Consider using Redis directly.");
        
        // In a real implementation, you would:
        // 1. Use StackExchange.Redis directly
        // 2. Use SCAN command with pattern
        // 3. Delete matching keys
        
        await Task.CompletedTask;
    }
}
