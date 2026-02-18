namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for sending real-time notifications via SignalR
/// </summary>
public interface ISignalRNotificationService
{
    /// <summary>
    /// Send notification to a specific user
    /// </summary>
    Task SendToUserAsync(Guid userId, string notificationType, object notification);
    
    /// <summary>
    /// Send notification to a group of users
    /// </summary>
    Task SendToGroupAsync(string groupName, string notificationType, object notification);
    
    /// <summary>
    /// Send notification to all users
    /// </summary>
    Task SendToAllAsync(string notificationType, object notification);
    
    /// <summary>
    /// Send appointment notification to user
    /// </summary>
    Task SendAppointmentNotificationAsync(Guid userId, string message, Guid appointmentId);
    
    /// <summary>
    /// Send prescription notification to user
    /// </summary>
    Task SendPrescriptionNotificationAsync(Guid userId, string message, Guid prescriptionId);
    
    /// <summary>
    /// Send QR code scan notification to user
    /// </summary>
    Task SendQRCodeScanNotificationAsync(Guid userId, string message, string qrCode);
}
