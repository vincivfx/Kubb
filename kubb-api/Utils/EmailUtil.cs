using System.Net.Mail;

namespace KubbAdminAPI.Utils;

public class EmailFactory
{
    public static MailMessage CreateEmailMessage(string receiver, string subject, string body)
    {
        var message = new MailMessage
        {
            To = { receiver },
            Body = body,
            Subject = subject,
            IsBodyHtml = true
        };

        return message;
    }
}