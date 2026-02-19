using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for pharmacist license verification and management
/// </summary>
public interface IPharmacistLicenseService
{
    /// <summary>
    /// Verify a pharmacist's license
    /// </summary>
    Task<LicenseVerificationResult> VerifyLicenseAsync(string licenseNumber);
    
    /// <summary>
    /// Get pharmacist profile with license details
    /// </summary>
    Task<PharmacistProfile?> GetPharmacistProfileAsync(Guid userId);
    
    /// <summary>
    /// Create or update pharmacist profile
    /// </summary>
    Task<PharmacistProfile> UpsertPharmacistProfileAsync(Guid userId, PharmacistProfileDto profileDto);
    
    /// <summary>
    /// Check if license is expired or expiring soon
    /// </summary>
    Task<List<PharmacistProfile>> GetExpiringLicensesAsync(int daysThreshold = 30);
    
    /// <summary>
    /// Update license status (for admin/verification workflow)
    /// </summary>
    Task<bool> UpdateLicenseStatusAsync(Guid pharmacistProfileId, PharmacistLicenseStatus newStatus, string? notes = null);
    
    /// <summary>
    /// Send license expiry reminders
    /// </summary>
    Task SendExpiryRemindersAsync();
}

/// <summary>
/// License verification result
/// </summary>
public class LicenseVerificationResult
{
    public bool IsValid { get; set; }
    public PharmacistLicenseStatus Status { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Message { get; set; }
    public DateTime VerifiedAt { get; set; }
}

/// <summary>
/// DTO for creating/updating pharmacist profile
/// </summary>
public class PharmacistProfileDto
{
    public required string LicenseNumber { get; set; }
    public string? LicenseAuthority { get; set; }
    public DateTime? LicenseIssueDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public int YearsOfExperience { get; set; }
    public List<string>? Education { get; set; }
    public List<string>? Certifications { get; set; }
    public List<string>? Languages { get; set; }
    public List<string>? Specializations { get; set; }
    public string? Biography { get; set; }
    public Guid? PrimaryPharmacyId { get; set; }
    public bool EnableExpiryReminders { get; set; } = true;
    public int ReminderDaysBeforeExpiry { get; set; } = 30;
}
