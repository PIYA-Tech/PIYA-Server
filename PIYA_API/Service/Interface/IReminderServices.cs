using PIYA_API.Model;

namespace PIYA_API.Service.Interface;

/// <summary>
/// Service for managing appointment reminders
/// </summary>
public interface IAppointmentReminderService
{
    /// <summary>
    /// Create reminder(s) for an appointment
    /// </summary>
    Task<List<AppointmentReminder>> CreateAppointmentRemindersAsync(Guid appointmentId, Guid userId, 
        List<int> minutesBeforeList, List<ReminderDeliveryMethod> deliveryMethods, string? customMessage = null);
    
    /// <summary>
    /// Get reminders for an appointment
    /// </summary>
    Task<List<AppointmentReminder>> GetAppointmentRemindersAsync(Guid appointmentId);
    
    /// <summary>
    /// Get pending reminders (due to be sent)
    /// </summary>
    Task<List<AppointmentReminder>> GetPendingRemindersAsync(DateTime? upToTime = null);
    
    /// <summary>
    /// Mark reminder as sent
    /// </summary>
    Task MarkReminderAsSentAsync(Guid reminderId, string? deliveryStatus = null);
    
    /// <summary>
    /// Cancel reminders for an appointment
    /// </summary>
    Task<int> CancelAppointmentRemindersAsync(Guid appointmentId);
    
    /// <summary>
    /// Process and send pending reminders
    /// </summary>
    Task<int> ProcessPendingRemindersAsync();
}

/// <summary>
/// Service for managing prescription refill reminders
/// </summary>
public interface IPrescriptionRefillReminderService
{
    /// <summary>
    /// Create a refill reminder for a prescription
    /// </summary>
    Task<PrescriptionRefillReminder> CreateRefillReminderAsync(Guid prescriptionId, Guid patientId, 
        DateTime estimatedRefillDate, int daysBeforeRefill, List<ReminderDeliveryMethod> deliveryMethods, 
        List<Guid>? medicationItemIds = null);
    
    /// <summary>
    /// Get reminders for a prescription
    /// </summary>
    Task<PrescriptionRefillReminder?> GetPrescriptionReminderAsync(Guid prescriptionId);
    
    /// <summary>
    /// Get user's refill reminders
    /// </summary>
    Task<List<PrescriptionRefillReminder>> GetUserRefillRemindersAsync(Guid userId, bool includeCompleted = false);
    
    /// <summary>
    /// Get pending refill reminders (due to be sent)
    /// </summary>
    Task<List<PrescriptionRefillReminder>> GetPendingRefillRemindersAsync(DateTime? upToDate = null);
    
    /// <summary>
    /// Mark reminder as sent
    /// </summary>
    Task MarkRefillReminderAsSentAsync(Guid reminderId, string? deliveryStatus = null);
    
    /// <summary>
    /// Acknowledge a refill reminder
    /// </summary>
    Task AcknowledgeRefillReminderAsync(Guid reminderId, Guid userId);
    
    /// <summary>
    /// Mark reminder as refilled (prescription renewed)
    /// </summary>
    Task MarkAsRefilledAsync(Guid reminderId, Guid newPrescriptionId);
    
    /// <summary>
    /// Cancel a refill reminder
    /// </summary>
    Task<bool> CancelRefillReminderAsync(Guid reminderId, Guid userId);
    
    /// <summary>
    /// Process and send pending refill reminders
    /// </summary>
    Task<int> ProcessPendingRefillRemindersAsync();
    
    /// <summary>
    /// Auto-create refill reminders for new prescriptions
    /// </summary>
    Task AutoCreateRefillRemindersAsync(Guid prescriptionId);
}
