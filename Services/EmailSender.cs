using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
//using (var client = new SmtpClient(settings["SmtpServer"], int.Parse(settings["Port"]))
//{
//    Credentials = new NetworkCredential(
//        settings["Username"],
//        settings["Password"]),
//    EnableSsl = true,
//    DeliveryMethod = SmtpDeliveryMethod.Network,
//    UseDefaultCredentials = false,
//    Timeout = 10000
//};
public class EmailSender
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendNotificationAsync(string subject, string htmlContent)
    {
        try
        {
            var settings = _config.GetSection("EmailSettings");
            using var client = new SmtpClient(settings["SmtpServer"], int.Parse(settings["Port"]))
            {
                Credentials = new NetworkCredential(
                    settings["Username"],
                    settings["Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(
                    settings["Username"],
                    settings["FromName"]),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            mail.To.Add(settings["AdminEmail"]);

            await client.SendMailAsync(mail);
            _logger.LogInformation("Email sent to admin");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email sending failed");
            throw; // ћожно заменить на очередь повторных попыток
        }
    }
}