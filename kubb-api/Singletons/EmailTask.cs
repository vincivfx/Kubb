using System.Collections.Concurrent;
using System.Net.Mail;

namespace KubbAdminAPI.Singletons;

public class EmailTask
{
    private readonly ConcurrentQueue<MailMessage> _messagesQueue = new();
    
    public void EnqueueEmail(MailMessage mailMessage) => _messagesQueue.Enqueue(mailMessage);

    public MailMessage? DequeueEmail()
    {
        _messagesQueue.TryDequeue(out var mailMessage);
        return mailMessage;
    }

}