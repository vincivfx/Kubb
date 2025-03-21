using System.Net;
using System.Net.Mail;

namespace KubbAdminAPI.Models.Configuration;

public class SmtpConfiguration
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseAuthentication { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public SmtpClient GetSmtpClient()
    {
        var smtpClient = new SmtpClient
        {
            Host = Server,
            Port = Port,
            EnableSsl = EnableSsl,
            UseDefaultCredentials = false
        };

        if (UseAuthentication)
        {
            smtpClient.Credentials = new NetworkCredential
            {
                UserName = Username,
                Password = Password
            };
        }
        
        return smtpClient;
    }
}