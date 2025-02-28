using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;

namespace KubbAdminAPI.Utils;

public class EmailService
{
    private readonly ConcurrentQueue<MailMessage> _messages = new();
    
    private SmtpClient smtpClient;

    public void AddMessage(MailMessage message) => _messages.Enqueue(message);
    
    public ConcurrentQueue<MailMessage> GetQueue() => _messages;

    public EmailService()
    {
        
    }

    public void SendEmail(MailMessage message)
    {
        Console.WriteLine(message.Body);
    }
    
}