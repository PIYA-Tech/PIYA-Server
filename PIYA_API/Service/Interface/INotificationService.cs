namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for sending notifications via Email, SMS, and Push
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Send email notification
    /// </summary>
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
    
    /// <summary>
    /// Send SMS notification
    /// </summary>
    Task<bool> SendSmsAsync(string phoneNumber, string message);
    
    /// <summary>
    /// Send push notification
    /// </summary>
    Task<bool> SendPushNotificationAsync(Guid userId, string title, string message, Dictionary<string, string>? data = null);
    
    /// <summary>
    /// Send appointment confirmation email
    /// </summary>
    Task<bool> SendAppointmentConfirmationAsync(string toEmail, string patientName, DateTime appointmentDate, string doctorName, string hospitalName);
    
    /// <summary>
    /// Send appointment reminder
    /// </summary>
    Task<bool> SendAppointmentReminderAsync(string toEmail, string phoneNumber, string patientName, DateTime appointmentDate, string doctorName);
    
    /// <summary>
    /// Send prescription ready notification
    /// </summary>
    Task<bool> SendPrescriptionReadyAsync(string toEmail, string phoneNumber, string patientName, string pharmacyName);
    
    /// <summary>
    /// Send 2FA code via email
    /// </summary>
    Task<bool> Send2FACodeEmailAsync(string toEmail, string code);
    
    /// <summary>
    /// Send 2FA code via SMS
    /// </summary>
    Task<bool> Send2FACodeSmsAsync(string phoneNumber, string code);
    
    /// <summary>
    /// Send password reset email
    /// </summary>
    Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken);
    
    /// <summary>
    /// Send email verification
    /// </summary>
    Task<bool> SendEmailVerificationAsync(string toEmail, string verificationToken);
    
    /// <summary>
    /// Send low stock alert to pharmacy
    /// </summary>
    Task<bool> SendLowStockAlertAsync(string toEmail, string pharmacyName, List<string> medicationNames);
}
