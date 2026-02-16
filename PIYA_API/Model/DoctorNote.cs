namespace PIYA_API.Model;

/// <summary>
/// Doctor note status enumeration
/// </summary>
public enum DoctorNoteStatus
{
    Active = 1,
    Revoked = 2,
    Expired = 3
}

/// <summary>
/// Digital medical certificate/note with public QR verification
/// </summary>
public class DoctorNote
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Patient for whom the note is issued
    /// </summary>
    public Guid PatientId { get; set; }
    public User Patient { get; set; } = null!;
    
    /// <summary>
    /// Doctor who issued the note
    /// </summary>
    public Guid DoctorId { get; set; }
    public User Doctor { get; set; } = null!;
    
    /// <summary>
    /// Associated appointment (optional)
    /// </summary>
    public Guid? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    /// <summary>
    /// Title of the note (e.g., "Medical Excuse Note", "Work Absence Certificate")
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Summary/reason (optional, controlled visibility)
    /// </summary>
    public string? Summary { get; set; }
    
    /// <summary>
    /// Valid from date
    /// </summary>
    public DateTime ValidFrom { get; set; }
    
    /// <summary>
    /// Valid to date
    /// </summary>
    public DateTime ValidTo { get; set; }
    
    /// <summary>
    /// When the note was issued
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Note status
    /// </summary>
    public DoctorNoteStatus Status { get; set; } = DoctorNoteStatus.Active;
    
    /// <summary>
    /// Public verification token (hashed in DB)
    /// </summary>
    public required string PublicTokenHash { get; set; }
    
    /// <summary>
    /// When the note was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }
    
    /// <summary>
    /// Reason for revocation
    /// </summary>
    public string? RevocationReason { get; set; }
    
    /// <summary>
    /// Unique note number for reference
    /// </summary>
    public required string NoteNumber { get; set; }
    
    /// <summary>
    /// Whether to include diagnosis details in public view
    /// </summary>
    public bool IncludeSummaryInPublicView { get; set; } = false;
    
    /// <summary>
    /// Hospital/clinic name (optional)
    /// </summary>
    public string? ClinicName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
