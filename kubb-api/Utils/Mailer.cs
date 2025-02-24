using System.Net;
using System.Net.Mail;

namespace KubbAdminAPI.Utils;

public class Mailer
{

    private string SmtpHost { get; set; }
    private int SmtpPort { get; set; }
    private string SmtpUsername { get; set; }
    private string SmtpPassword { get; set; }
    private bool EnableSsl { get; set; }
    
    private SmtpClient? SmtpClient { get; set; }
    
    private string FromEmail { get; set; }
    private string FromName { get; set; }
    
    
    public Mailer(MailerBuilder builder)
    {
        SmtpHost = builder.SmtpHost;
        SmtpPort = builder.SmtpPort;
        SmtpUsername = builder.SmtpUsername;
        SmtpPassword = builder.SmtpPassword;
        FromEmail = builder.FromEmail;
        FromName = builder.FromName;
        EnableSsl = builder.EnableSsl;

        this.SmtpClient = new SmtpClient
        {
            Host = SmtpHost,
            Port = SmtpPort,
            UseDefaultCredentials = false,
            EnableSsl = EnableSsl,
            Credentials = new NetworkCredential(SmtpUsername, SmtpPassword)
        };
    }

    public Task SendAsync(string to, string subject, string body)
    {
        var message = new MailMessage
        {
            To = { to },
            From = new MailAddress(FromEmail, FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        return SmtpClient!.SendMailAsync(message);
    }

    
    
}

public class MailerBuilder
{

    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public bool EnableSsl { get; set; }
    
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    
    
    
    public Mailer Build()
    {
        return new Mailer(this);
    }
        
}