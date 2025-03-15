using System.Net;
using System.Net.Mail;

namespace KubbAdminAPI.Models.Configuration;

public class SmtpConfiguration
{
    public required string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public bool SmtpEnableSsl { get; set; }
    public bool SmtpUseAuthentication { get; set; }
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }

    public SmtpClient GetSmtpClient()
    {
        var smtpClient = new SmtpClient
        {
            Host = SmtpServer,
            Port = SmtpPort,
            EnableSsl = SmtpEnableSsl,
            UseDefaultCredentials = false
        };

        if (SmtpUseAuthentication)
        {
            smtpClient.Credentials = new NetworkCredential
            {
                UserName = SmtpUsername,
                Password = SmtpPassword
            };
        }
        
        return smtpClient;
    }
}