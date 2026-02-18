namespace PIYA_API.Model;

/// <summary>
/// Prescription status enumeration
/// </summary>
public enum PrescriptionStatus
{
    Active = 1,
    PartiallyFulfilled = 2,
    Fulfilled = 3,
    Expired = 4,
    Cancelled = 5
}

/// <summary>
/// Digital prescription from doctor to patient
/// </summary>
public class Prescription
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Patient for whom prescription is written
    /// </summary>
    public Guid PatientId { get; set; }
    public User Patient { get; set; } = null!;
    
    /// <summary>
    /// Doctor who wrote the prescription
    /// </summary>
    public Guid DoctorId { get; set; }
    public User Doctor { get; set; } = null!;
    
    /// <summary>
    /// Associated appointment (optional)
    /// </summary>
    public Guid? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    /// <summary>
    /// Prescription status
    /// </summary>
    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Active;
    
    /// <summary>
    /// Diagnosis or reason for prescription
    /// </summary>
    public string? Diagnosis { get; set; }
    
    /// <summary>
    /// General instructions for patient
    /// </summary>
    public string? Instructions { get; set; }
    
    /// <summary>
    /// Date prescription was issued
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// When prescription expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Digital signature/hash for verification
    /// </summary>
    public string? DigitalSignature { get; set; }
    
    /// <summary>
    /// QR token for pharmacy verification (time-limited)
    /// </summary>
    public string? QrToken { get; set; }
    
    /// <summary>
    /// QR token expiration (5 minutes from generation)
    /// </summary>
    public DateTime? QrTokenExpiresAt { get; set; }
    
    /// <summary>
    /// When prescription was fulfilled
    /// </summary>
    public DateTime? FulfilledAt { get; set; }
    
    /// <summary>
    /// Pharmacy that fulfilled the prescription
    /// </summary>
    public Guid? FulfilledByPharmacyId { get; set; }
    public Pharmacy? FulfilledByPharmacy { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<PrescriptionItem> Items { get; set; } = [];
}
