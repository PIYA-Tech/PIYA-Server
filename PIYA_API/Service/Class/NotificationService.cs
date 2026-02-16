using PIYA_API.Service.Interface;

namespace PIYA_API.Service.Class;

public class NotificationService(IConfiguration configuration, ILogger<NotificationService> logger) : INotificationService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<NotificationService> _logger = logger;

    public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            // TODO: Integrate with SendGrid or similar email service
            // var apiKey = _configuration["Notification:SendGrid:ApiKey"];
            // var client = new SendGridClient(apiKey);
            // var from = new EmailAddress(_configuration["Notification:SendGrid:FromEmail"], "PIYA Healthcare");
            // var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, body, isHtml ? body : null);
            // var response = await client.SendEmailAsync(msg);
            // return response.IsSuccessStatusCode;

            _logger.LogInformation($"[EMAIL] To: {to}, Subject: {subject}");
            _logger.LogDebug($"[EMAIL] Body: {body}");
            
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {to}");
            return false;
        }
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            // TODO: Integrate with Twilio or similar SMS service
            // var accountSid = _configuration["Notification:Twilio:AccountSid"];
            // var authToken = _configuration["Notification:Twilio:AuthToken"];
            // var fromNumber = _configuration["Notification:Twilio:FromNumber"];
            // TwilioClient.Init(accountSid, authToken);
            // var messageResource = await MessageResource.CreateAsync(
            //     body: message,
            //     from: new PhoneNumber(fromNumber),
            //     to: new PhoneNumber(phoneNumber)
            // );
            // return messageResource.Status != MessageResource.StatusEnum.Failed;

            _logger.LogInformation($"[SMS] To: {phoneNumber}, Message: {message}");
            
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send SMS to {phoneNumber}");
            return false;
        }
    }

    public async Task<bool> SendPushNotificationAsync(Guid userId, string title, string body, Dictionary<string, string>? data = null)
    {
        try
        {
            // TODO: Integrate with Firebase Cloud Messaging (FCM)
            // Fetch user's device token from database
            // var user = await _context.Users.FindAsync(userId);
            // var deviceToken = user.DeviceToken;
            
            // var serverKey = _configuration["Notification:FCM:ServerKey"];
            // var message = new
            // {
            //     to = deviceToken,
            //     notification = new { title, body },
            //     data = data ?? new Dictionary<string, string>()
            // };
            // var httpClient = new HttpClient();
            // httpClient.DefaultRequestHeaders.Add("Authorization", $"key={serverKey}");
            // var response = await httpClient.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", message);
            // return response.IsSuccessStatusCode;

            _logger.LogInformation($"[PUSH] User: {userId}, Title: {title}, Body: {body}");
            
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send push notification to user {userId}");
            return false;
        }
    }

    public async Task<bool> SendAppointmentConfirmationAsync(string email, string patientName, DateTime appointmentTime, string doctorName, string hospitalName)
    {
        var subject = "Appointment Confirmation - PIYA Healthcare";
        var body = $@"
            <h2>Appointment Confirmed</h2>
            <p>Dear {patientName},</p>
            <p>Your appointment has been confirmed with the following details:</p>
            <ul>
                <li><strong>Doctor:</strong> {doctorName}</li>
                <li><strong>Hospital:</strong> {hospitalName}</li>
                <li><strong>Date & Time:</strong> {appointmentTime:dddd, MMMM dd, yyyy 'at' h:mm tt}</li>
            </ul>
            <p>Please arrive 10 minutes early for registration.</p>
            <p>If you need to reschedule or cancel, please contact us at least 24 hours in advance.</p>
            <br>
            <p>Best regards,<br>PIYA Healthcare Team</p>
        ";

        return await SendEmailAsync(email, subject, body, true);
    }

    public async Task<bool> SendAppointmentReminderAsync(string email, string phoneNumber, string patientName, DateTime appointmentTime, string doctorName)
    {
        var hoursUntil = (appointmentTime - DateTime.UtcNow).TotalHours;
        
        var emailTask = SendEmailAsync(
            email,
            "Appointment Reminder",
            $@"
                <h2>Appointment Reminder</h2>
                <p>Dear {patientName},</p>
                <p>This is a reminder that you have an appointment with Dr. {doctorName} in {Math.Round(hoursUntil)} hours.</p>
                <p><strong>Date & Time:</strong> {appointmentTime:dddd, MMMM dd, yyyy 'at' h:mm tt}</p>
                <p>Please arrive 10 minutes early.</p>
                <br>
                <p>Best regards,<br>PIYA Healthcare Team</p>
            ",
            true
        );
        
        var smsTask = SendSmsAsync(
            phoneNumber,
            $"Reminder: You have an appointment with Dr. {doctorName} in {Math.Round(hoursUntil)} hours at {appointmentTime:h:mm tt}. - PIYA Healthcare"
        );

        var results = await Task.WhenAll(emailTask, smsTask);
        return results.Any(r => r);
    }

    public async Task<bool> SendPrescriptionReadyAsync(string email, string phoneNumber, string patientName, string pharmacyName)
    {
        var emailTask = SendEmailAsync(
            email,
            "Your Prescription is Ready",
            $@"
                <h2>Prescription Ready for Pickup</h2>
                <p>Dear {patientName},</p>
                <p>Your prescription is now ready for pickup at <strong>{pharmacyName}</strong>.</p>
                <p>Please bring your ID and insurance card when picking up your medication.</p>
                <p>For questions, please contact the pharmacy directly.</p>
                <br>
                <p>Best regards,<br>PIYA Healthcare Team</p>
            ",
            true
        );

        var smsTask = SendSmsAsync(
            phoneNumber,
            $"Your prescription is ready for pickup at {pharmacyName}. Bring your ID. - PIYA Healthcare"
        );

        var results = await Task.WhenAll(emailTask, smsTask);
        return results.Any(r => r); // Success if at least one channel succeeds
    }

    public async Task<bool> Send2FACodeEmailAsync(string email, string code)
    {
        var subject = "Your Two-Factor Authentication Code";
        var body = $@"
            <h2>Two-Factor Authentication</h2>
            <p>Your verification code is:</p>
            <h1 style='font-size: 32px; letter-spacing: 5px; font-family: monospace;'>{code}</h1>
            <p>This code will expire in 5 minutes.</p>
            <p>If you didn't request this code, please ignore this email or contact support if you have concerns.</p>
            <br>
            <p>Best regards,<br>PIYA Healthcare Security Team</p>
        ";

        return await SendEmailAsync(email, subject, body, true);
    }

    public async Task<bool> Send2FACodeSmsAsync(string phoneNumber, string code)
    {
        var message = $"Your PIYA Healthcare verification code is: {code}. Valid for 5 minutes. Do not share this code.";
        return await SendSmsAsync(phoneNumber, message);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email, string resetToken)
    {
        var resetLink = $"{_configuration["App:BaseUrl"]}/reset-password?token={resetToken}";
        
        var subject = "Password Reset Request";
        var body = $@"
            <h2>Password Reset Request</h2>
            <p>You requested to reset your password for your PIYA Healthcare account.</p>
            <p>Click the link below to reset your password:</p>
            <p><a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>
            <p>This link will expire in 1 hour.</p>
            <p>If you didn't request a password reset, please ignore this email and ensure your account is secure.</p>
            <br>
            <p>Best regards,<br>PIYA Healthcare Security Team</p>
        ";

        return await SendEmailAsync(email, subject, body, true);
    }

    public async Task<bool> SendEmailVerificationAsync(string email, string verificationToken)
    {
        var verificationLink = $"{_configuration["App:BaseUrl"]}/verify-email?token={verificationToken}";
        
        var subject = "Verify Your Email Address";
        var body = $@"
            <h2>Welcome to PIYA Healthcare!</h2>
            <p>Thank you for registering. Please verify your email address to activate your account.</p>
            <p>Click the link below to verify:</p>
            <p><a href='{verificationLink}' style='display: inline-block; padding: 10px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px;'>Verify Email</a></p>
            <p>This link will expire in 24 hours.</p>
            <p>If you didn't create this account, please ignore this email.</p>
            <br>
            <p>Best regards,<br>PIYA Healthcare Team</p>
        ";

        return await SendEmailAsync(email, subject, body, true);
    }

    public async Task<bool> SendLowStockAlertAsync(string pharmacyEmail, string pharmacyName, List<string> medicationNames)
    {
        var subject = $"Low Stock Alert - {pharmacyName}";
        var medicationList = string.Join("<br>", medicationNames.Select(m => $"• {m}"));
        
        var body = $@"
            <h2>Low Stock Alert</h2>
            <p>This is an automated alert from your pharmacy inventory system.</p>
            <p><strong>Pharmacy:</strong> {pharmacyName}</p>
            <p><strong>Low Stock Medications:</strong></p>
            <p>{medicationList}</p>
            <p style='color: red;'><strong>Action Required:</strong> Please restock these medications as soon as possible.</p>
            <br>
            <p>PIYA Healthcare Inventory Management System</p>
        ";

        return await SendEmailAsync(pharmacyEmail, subject, body, true);
    }
}
