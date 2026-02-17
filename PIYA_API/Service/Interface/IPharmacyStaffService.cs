using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for managing pharmacy staff assignments
/// </summary>
public interface IPharmacyStaffService
{
    #region Staff Assignment
    
    /// <summary>
    /// Assign a user to a pharmacy as staff
    /// </summary>
    Task<PharmacyStaff> AssignStaffAsync(Guid pharmacyId, Guid userId, PharmacyStaffRole role, Guid assignedByUserId);
    
    /// <summary>
    /// Remove staff member from pharmacy
    /// </summary>
    Task<bool> RemoveStaffAsync(Guid pharmacyId, Guid userId);
    
    /// <summary>
    /// Update staff member's role or details
    /// </summary>
    Task<PharmacyStaff> UpdateStaffAsync(Guid staffId, PharmacyStaffRole? newRole = null, string? workSchedule = null, List<string>? permissions = null);
    
    #endregion
    
    #region Staff Queries
    
    /// <summary>
    /// Get all staff members for a pharmacy
    /// </summary>
    Task<List<PharmacyStaff>> GetPharmacyStaffAsync(Guid pharmacyId, bool activeOnly = true);
    
    /// <summary>
    /// Get pharmacies where a user is staff
    /// </summary>
    Task<List<PharmacyStaff>> GetUserPharmaciesAsync(Guid userId, bool activeOnly = true);
    
    /// <summary>
    /// Get staff assignment by ID
    /// </summary>
    Task<PharmacyStaff?> GetStaffAssignmentAsync(Guid staffId);
    
    /// <summary>
    /// Check if user is staff at pharmacy
    /// </summary>
    Task<bool> IsStaffAtPharmacyAsync(Guid pharmacyId, Guid userId);
    
    /// <summary>
    /// Check if user is manager at pharmacy
    /// </summary>
    Task<bool> IsManagerAtPharmacyAsync(Guid pharmacyId, Guid userId);
    
    /// <summary>
    /// Get manager of a pharmacy
    /// </summary>
    Task<PharmacyStaff?> GetPharmacyManagerAsync(Guid pharmacyId);
    
    #endregion
    
    #region Manager Operations
    
    /// <summary>
    /// Assign manager to pharmacy
    /// </summary>
    Task<PharmacyStaff> AssignManagerAsync(Guid pharmacyId, Guid userId, Guid assignedByUserId);
    
    /// <summary>
    /// Transfer pharmacy management to new user
    /// </summary>
    Task<PharmacyStaff> TransferManagementAsync(Guid pharmacyId, Guid newManagerUserId, Guid performedByUserId);
    
    #endregion
}
