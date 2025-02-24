using System.Net;
using System.Net.Mail;

namespace KubbAdminAPI.Utils;

public class EmailService
{
    // Configuration property to access application settings.
    private readonly IConfiguration _configuration;

    private string SmtpServer { get; set; }
    private string? SmtpUser { get; set; }
    private string? SmtpPassword { get; set; }
    private bool SmtpEnableSSL { get; set; } = false;
    private int SmtpPort { get; set; }
    
    
    private SmtpClient? SmtpClient { get; set; }
    
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        SmtpServer = _configuration["EmailSettings:SmtpServer"]!;
        SmtpUser = _configuration["EmailSettings:SmtpUsername"]!;
        SmtpPassword = _configuration["EmailSettings:SmtpPassword"]!;
        SmtpEnableSSL = _configuration["EmailSettings:SmtpEnableSSL"] == "true";
        SmtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!);

        SmtpClient = new SmtpClient
        {
            Host = SmtpServer,
            Credentials = new NetworkCredential(SmtpUser, SmtpPassword),
            EnableSsl = SmtpEnableSSL,
            Port = SmtpPort
        };

    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new MailMessage
        {
            To = { email },
            Body = body,
            Subject = subject,
            IsBodyHtml = true,
        };
        await this.SmtpClient!.SendMailAsync(message);
    }
    
    
}