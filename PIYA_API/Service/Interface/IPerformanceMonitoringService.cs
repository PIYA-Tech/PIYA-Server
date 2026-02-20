namespace PIYA_API.Service.Interface;

/// <summary>
/// Performance monitoring and optimization service
/// </summary>
public interface IPerformanceMonitoringService
{
    /// <summary>
    /// Record API endpoint performance metrics
    /// </summary>
    Task RecordEndpointMetricAsync(string endpoint, string method, int statusCode, long durationMs, long? memoryUsed = null);
    
    /// <summary>
    /// Get performance metrics for specific endpoint
    /// </summary>
    Task<EndpointPerformanceMetrics> GetEndpointMetricsAsync(string endpoint, DateTime? startDate = null, DateTime? endDate = null);
    
    /// <summary>
    /// Get slowest endpoints
    /// </summary>
    Task<List<EndpointPerformanceMetrics>> GetSlowestEndpointsAsync(int top = 10);
    
    /// <summary>
    /// Get database query performance metrics
    /// </summary>
    Task<List<DatabaseQueryMetric>> GetDatabaseMetricsAsync(int top = 20);
    
    /// <summary>
    /// Get cache hit/miss statistics
    /// </summary>
    Task<CacheStatistics> GetCacheStatisticsAsync();
    
    /// <summary>
    /// Get system resource usage
    /// </summary>
    Task<SystemResourceMetrics> GetSystemResourcesAsync();
    
    /// <summary>
    /// Clear performance metrics older than specified days
    /// </summary>
    Task<int> CleanupOldMetricsAsync(int olderThanDays = 30);
}

#region DTOs

public class EndpointPerformanceMetrics
{
    public required string Endpoint { get; set; }
    public required string Method { get; set; }
    public long TotalRequests { get; set; }
    public long SuccessfulRequests { get; set; }
    public long FailedRequests { get; set; }
    public double AverageDurationMs { get; set; }
    public long MinDurationMs { get; set; }
    public long MaxDurationMs { get; set; }
    public double P50DurationMs { get; set; } // Median
    public double P95DurationMs { get; set; }
    public double P99DurationMs { get; set; }
    public double ErrorRate { get; set; }
    public DateTime FirstRequestAt { get; set; }
    public DateTime LastRequestAt { get; set; }
}

public class DatabaseQueryMetric
{
    public required string QueryType { get; set; }
    public required string TableName { get; set; }
    public long ExecutionCount { get; set; }
    public double AverageDurationMs { get; set; }
    public long TotalDurationMs { get; set; }
    public double CacheHitRate { get; set; }
}

public class CacheStatistics
{
    public long TotalRequests { get; set; }
    public long CacheHits { get; set; }
    public long CacheMisses { get; set; }
    public double HitRate { get; set; }
    public long TotalKeys { get; set; }
    public long ExpiredKeys { get; set; }
    public long MemoryUsedBytes { get; set; }
    public double AverageGetTimeMs { get; set; }
}

public class SystemResourceMetrics
{
    public double CpuUsagePercent { get; set; }
    public long MemoryUsedBytes { get; set; }
    public long MemoryTotalBytes { get; set; }
    public double MemoryUsagePercent { get; set; }
    public long DiskUsedBytes { get; set; }
    public long DiskTotalBytes { get; set; }
    public int ActiveConnections { get; set; }
    public int ThreadPoolThreads { get; set; }
    public DateTime MeasuredAt { get; set; }
}

#endregion
