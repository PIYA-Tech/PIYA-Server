namespace PIYA_API.Model;

/// <summary>
/// Audit log for tracking all healthcare-related transactions and security events
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// User who performed the action
    /// </summary>
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    /// <summary>
    /// Action type (e.g., "Login", "CreatePrescription", "UpdateUser", "DeletePharmacy")
    /// </summary>
    public required string Action { get; set; }
    
    /// <summary>
    /// Entity type affected (e.g., "User", "Prescription", "Pharmacy")
    /// </summary>
    public string? EntityType { get; set; }
    
    /// <summary>
    /// Entity ID affected
    /// </summary>
    public string? EntityId { get; set; }
    
    /// <summary>
    /// Detailed description of the action
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// IP address of the request
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent (browser/device info)
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// HTTP method (GET, POST, PUT, DELETE)
    /// </summary>
    public string? HttpMethod { get; set; }
    
    /// <summary>
    /// Request endpoint
    /// </summary>
    public string? Endpoint { get; set; }
    
    /// <summary>
    /// Response status code
    /// </summary>
    public int? StatusCode { get; set; }
    
    /// <summary>
    /// Additional metadata (JSON format)
    /// </summary>
    public string? Metadata { get; set; }
    
    /// <summary>
    /// Timestamp when the action occurred
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Whether the action was successful
    /// </summary>
    public bool IsSuccess { get; set; } = true;
    
    /// <summary>
    /// Error message if action failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
