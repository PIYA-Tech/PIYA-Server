namespace PIYA_API.Model;

/// <summary>
/// Medical document metadata
/// </summary>
public class MedicalDocument
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Associated user (patient/doctor)
    /// </summary>
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Document type
    /// </summary>
    public MedicalDocumentType DocumentType { get; set; }
    
    /// <summary>
    /// Original filename
    /// </summary>
    public required string FileName { get; set; }
    
    /// <summary>
    /// Stored filename (UUID-based)
    /// </summary>
    public required string StoredFileName { get; set; }
    
    /// <summary>
    /// File path relative to storage root
    /// </summary>
    public required string FilePath { get; set; }
    
    /// <summary>
    /// MIME type
    /// </summary>
    public required string ContentType { get; set; }
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSizeBytes { get; set; }
    
    /// <summary>
    /// Document title/description
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Related appointment ID
    /// </summary>
    public Guid? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    /// <summary>
    /// Related prescription ID
    /// </summary>
    public Guid? PrescriptionId { get; set; }
    public Prescription? Prescription { get; set; }
    
    /// <summary>
    /// Uploaded by user ID (could be different from owner)
    /// </summary>
    public Guid UploadedByUserId { get; set; }
    public User? UploadedBy { get; set; }
    
    /// <summary>
    /// Upload timestamp
    /// </summary>
    public DateTime UploadedAt { get; set; }
    
    /// <summary>
    /// Last modified timestamp
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
    
    /// <summary>
    /// Whether the document is verified by a doctor
    /// </summary>
    public bool IsVerified { get; set; } = false;
    
    /// <summary>
    /// Verified by doctor ID
    /// </summary>
    public Guid? VerifiedByUserId { get; set; }
    public User? VerifiedBy { get; set; }
    
    /// <summary>
    /// Verification timestamp
    /// </summary>
    public DateTime? VerifiedAt { get; set; }
    
    /// <summary>
    /// File hash for integrity verification (SHA-256)
    /// </summary>
    public string? FileHash { get; set; }
    
    /// <summary>
    /// Whether the document is archived/deleted
    /// </summary>
    public bool IsArchived { get; set; } = false;
    
    /// <summary>
    /// Archive timestamp
    /// </summary>
    public DateTime? ArchivedAt { get; set; }
}

/// <summary>
/// Medical document types
/// </summary>
public enum MedicalDocumentType
{
    LabReport,
    XRay,
    MRI,
    CTScan,
    Ultrasound,
    Prescription,
    MedicalCertificate,
    DischargeSummary,
    VaccinationRecord,
    AllergyCard,
    InsuranceCard,
    IdDocument,
    Other
}
