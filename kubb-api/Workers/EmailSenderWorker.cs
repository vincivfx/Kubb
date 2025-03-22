using KubbAdminAPI.Models.Configuration;
using KubbAdminAPI.Singletons;

namespace KubbAdminAPI.Workers;

/**
 * MISSION: background worker enabled to send emails that are enqueued in EmailTask
 */
public class EmailSenderWorker(ILogger<EmailSenderWorker> logger, EmailTask emailTask, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configSection = configuration.GetSection("EmailSettings");
        var config = configSection.Get<SmtpConfiguration>();

        var smtpClient = config!.GetSmtpClient();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var mailMessage = emailTask.DequeueEmail();
            if (mailMessage != null)
            {
                logger.LogInformation($"Processing work item: {mailMessage.To}");
                smtpClient.Send(mailMessage);
            }
            else
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}