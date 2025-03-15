using System.Net.Mail;

namespace KubbAdminAPI.Utils;

public abstract class EmailFactory
{
    public static MailMessage CreateEmailMessage(string receiver, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress("kcp-noreply@noreply.vincivfx.it"),
            To = { receiver },
            Body = body,
            Subject = subject,
            IsBodyHtml = true
        };

        return message;
    }
}