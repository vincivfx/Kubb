using System.Collections.Concurrent;
using System.Net.Mail;

namespace KubbAdminAPI.Services;

public class EmailService
{
    private readonly ConcurrentQueue<MailMessage> _messages = new();

    public void AddMessage(MailMessage message) => _messages.Enqueue(message);
    
    public ConcurrentQueue<MailMessage> GetQueue() => _messages;

    public void EnqueueEmail(MailMessage message)
    {
        _messages.Enqueue(message);
        Console.WriteLine(message.Body);
    }
    
}