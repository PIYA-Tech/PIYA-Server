namespace PIYA_API.Service.Interface;

public interface ICalendarService
{
    /// <summary>
    /// Generates an iCal/ICS file content for an appointment
    /// </summary>
    Task<string> GenerateAppointmentICalAsync(Guid appointmentId);
    
    /// <summary>
    /// Generates an iCal/ICS file content with custom event details
    /// </summary>
    string GenerateICalEvent(
        string summary,
        string description,
        DateTime startTime,
        DateTime endTime,
        string location,
        string organizerEmail,
        string organizerName,
        string attendeeEmail,
        string attendeeName
    );
    
    /// <summary>
    /// Generates a Google Calendar URL for an appointment
    /// </summary>
    Task<string> GenerateGoogleCalendarUrlAsync(Guid appointmentId);
    
    /// <summary>
    /// Generates an Outlook Calendar URL for an appointment
    /// </summary>
    Task<string> GenerateOutlookCalendarUrlAsync(Guid appointmentId);
    
    /// <summary>
    /// Sends calendar invitation email with ICS attachment
    /// </summary>
    Task<bool> SendCalendarInvitationAsync(Guid appointmentId, string recipientEmail);
}
