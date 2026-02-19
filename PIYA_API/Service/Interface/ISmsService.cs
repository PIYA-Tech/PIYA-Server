namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for sending SMS messages via Twilio
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// Send a generic SMS message
    /// </summary>
    Task<bool> SendSmsAsync(string toPhoneNumber, string message);
    
    /// <summary>
    /// Send verification code SMS
    /// </summary>
    Task<bool> SendVerificationCodeAsync(string toPhoneNumber, string code);
    
    /// <summary>
    /// Send appointment reminder SMS
    /// </summary>
    Task<bool> SendAppointmentReminderAsync(string toPhoneNumber, DateTime appointmentTime, string doctorName, string hospitalName);
    
    /// <summary>
    /// Send prescription ready notification
    /// </summary>
    Task<bool> SendPrescriptionReadyAsync(string toPhoneNumber, string pharmacyName, string pharmacyAddress);
    
    /// <summary>
    /// Send prescription refill reminder
    /// </summary>
    Task<bool> SendRefillReminderAsync(string toPhoneNumber, string medicationName, DateTime refillDate);
    
    /// <summary>
    /// Send 2FA code
    /// </summary>
    Task<bool> Send2FACodeAsync(string toPhoneNumber, string code);
    
    /// <summary>
    /// Send password reset code
    /// </summary>
    Task<bool> SendPasswordResetCodeAsync(string toPhoneNumber, string code);
}
