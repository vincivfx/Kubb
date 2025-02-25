using System.Collections.Concurrent;
using System.Net.Mail;

namespace KubbAdminAPI.Workers;

public class EmailSenderWorker(ILogger<EmailSenderWorker> logger) : BackgroundService
{
    private readonly ConcurrentQueue<MailMessage> _workItems = new ConcurrentQueue<MailMessage>();
    
    public void QueueBackgroundWorkItem(MailMessage message)
    {
        _workItems.Enqueue(message);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_workItems.TryDequeue(out var workItem))
            {
                logger.LogInformation($"Processing work item: {workItem.To}");
                await Task.Delay(1000, stoppingToken);
            }
            else
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}