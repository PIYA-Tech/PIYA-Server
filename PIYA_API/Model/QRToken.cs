namespace PIYA_API.Model;

/// <summary>
/// QR token for secure prescription verification
/// Implements anti-replay attack prevention and audit trail
/// </summary>
public class QRToken
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Token hash (SHA256 of the actual token for security)
    /// </summary>
    public string TokenHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Type of entity this QR code is for (Prescription, DoctorNote, etc.)
    /// </summary>
    public string EntityType { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the entity (PrescriptionId, DoctorNoteId, etc.)
    /// </summary>
    public Guid EntityId { get; set; }
    
    /// <summary>
    /// User who generated the QR code (PatientId)
    /// </summary>
    public Guid GeneratedByUserId { get; set; }
    public User GeneratedByUser { get; set; } = null!;
    
    /// <summary>
    /// When the token was generated
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// When the token expires (5 minutes from generation)
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Whether this token has been used (one-time use enforcement)
    /// </summary>
    public bool IsUsed { get; set; } = false;
    
    /// <summary>
    /// When the token was used
    /// </summary>
    public DateTime? UsedAt { get; set; }
    
    /// <summary>
    /// User who used/scanned the QR code (PharmacistId)
    /// </summary>
    public Guid? UsedByUserId { get; set; }
    public User? UsedByUser { get; set; }
    
    /// <summary>
    /// Whether this token has been manually revoked
    /// </summary>
    public bool IsRevoked { get; set; } = false;
    
    /// <summary>
    /// When the token was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }
    
    /// <summary>
    /// User who revoked the token
    /// </summary>
    public Guid? RevokedByUserId { get; set; }
    public User? RevokedByUser { get; set; }
    
    /// <summary>
    /// Reason for revocation
    /// </summary>
    public string? RevocationReason { get; set; }
    
    /// <summary>
    /// IP address from which the token was generated
    /// </summary>
    public string? GeneratedFromIp { get; set; }
    
    /// <summary>
    /// IP address from which the token was used
    /// </summary>
    public string? UsedFromIp { get; set; }
    
    /// <summary>
    /// Device/User-Agent that generated the token
    /// </summary>
    public string? GeneratedFromDevice { get; set; }
    
    /// <summary>
    /// Device/User-Agent that used the token
    /// </summary>
    public string? UsedFromDevice { get; set; }
    
    /// <summary>
    /// Number of validation attempts
    /// </summary>
    public int ValidationAttempts { get; set; } = 0;
    
    /// <summary>
    /// Last validation attempt timestamp
    /// </summary>
    public DateTime? LastValidationAttempt { get; set; }
}

/// <summary>
/// QR token status for queries
/// </summary>
public enum QRTokenStatus
{
    Active = 1,      // Valid and not used
    Used = 2,        // Successfully used (one-time)
    Expired = 3,     // Past expiration time
    Revoked = 4      // Manually revoked
}
