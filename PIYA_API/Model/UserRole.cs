namespace PIYA_API.Model;

/// <summary>
/// User roles for role-based access control (RBAC)
/// </summary>
public enum UserRole
{
    Patient = 1,
    Doctor = 2,
    Pharmacist = 3,
    PharmacyManager = 4,
    Admin = 5,
    SuperAdmin = 6
}
