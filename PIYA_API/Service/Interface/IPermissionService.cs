using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for managing user permissions and access control
/// </summary>
public interface IPermissionService
{
    #region Permission Management
    
    /// <summary>
    /// Grant a permission to a user
    /// </summary>
    Task<UserPermission> GrantPermissionAsync(Guid userId, string permission, Guid grantedByUserId, string? resourceId = null, DateTime? expiresAt = null);
    
    /// <summary>
    /// Revoke a permission from a user
    /// </summary>
    Task<bool> RevokePermissionAsync(Guid permissionId);
    
    /// <summary>
    /// Revoke all permissions for a user
    /// </summary>
    Task<int> RevokeAllUserPermissionsAsync(Guid userId);
    
    /// <summary>
    /// Revoke specific permission type for a user
    /// </summary>
    Task<int> RevokeUserPermissionByTypeAsync(Guid userId, string permission);
    
    #endregion
    
    #region Permission Queries
    
    /// <summary>
    /// Get all permissions for a user
    /// </summary>
    Task<List<UserPermission>> GetUserPermissionsAsync(Guid userId, bool activeOnly = true);
    
    /// <summary>
    /// Check if user has a specific permission
    /// </summary>
    Task<bool> HasPermissionAsync(Guid userId, string permission, string? resourceId = null);
    
    /// <summary>
    /// Check if user has any of the specified permissions
    /// </summary>
    Task<bool> HasAnyPermissionAsync(Guid userId, params string[] permissions);
    
    /// <summary>
    /// Check if user has all of the specified permissions
    /// </summary>
    Task<bool> HasAllPermissionsAsync(Guid userId, params string[] permissions);
    
    /// <summary>
    /// Get all users with a specific permission
    /// </summary>
    Task<List<User>> GetUsersWithPermissionAsync(string permission);
    
    #endregion
    
    #region Role-Based Permissions
    
    /// <summary>
    /// Grant default permissions based on user role
    /// </summary>
    Task<List<UserPermission>> GrantRolePermissionsAsync(Guid userId, UserRole role, Guid grantedByUserId);
    
    /// <summary>
    /// Get default permissions for a role
    /// </summary>
    List<string> GetRoleDefaultPermissions(UserRole role);
    
    /// <summary>
    /// Check if user has permission based on role or explicit permission
    /// </summary>
    Task<bool> HasPermissionOrRoleAsync(Guid userId, string permission, UserRole requiredRole);
    
    #endregion
    
    #region Admin Dashboard Access
    
    /// <summary>
    /// Check if user can access admin dashboard
    /// </summary>
    Task<bool> CanAccessAdminDashboardAsync(Guid userId);
    
    /// <summary>
    /// Check if user can manage other users
    /// </summary>
    Task<bool> CanManageUsersAsync(Guid userId);
    
    /// <summary>
    /// Check if user can view audit logs
    /// </summary>
    Task<bool> CanViewAuditLogsAsync(Guid userId);
    
    /// <summary>
    /// Check if user can modify system settings
    /// </summary>
    Task<bool> CanModifySystemSettingsAsync(Guid userId);
    
    #endregion
}
