using System.Collections.Concurrent;
using System.Net.Mail;
using KubbAdminAPI.Utils;

namespace KubbAdminAPI.Workers;

public class EmailSenderWorker(ILogger<EmailSenderWorker> logger, IServiceScopeFactory _serviceScopeFactory) : BackgroundService
{
    private ConcurrentQueue<MailMessage> _workItems;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        using var scope = _serviceScopeFactory.CreateScope();
        var emailService = scope.ServiceProvider.GetService<EmailService>();

        this._workItems = emailService!.GetQueue();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_workItems.TryDequeue(out var workItem))
            {
                logger.LogInformation($"Processing work item: {workItem.To}");
                await Task.Run(() => emailService.SendEmail(workItem), stoppingToken);
            }
            else
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}