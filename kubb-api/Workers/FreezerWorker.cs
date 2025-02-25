using System.Collections.Concurrent;
using System.Net.Mail;
using KubbAdminAPI.Models.RequestModels.ChallengeAdmin;

namespace KubbAdminAPI.Workers;

public class FreezerWorker : BackgroundService
{
    private readonly ConcurrentQueue<Challenge> _workItems = new ConcurrentQueue<Challenge>();

    public void FreezeChallenge(Challenge challenge)
    {
        _workItems.Enqueue(challenge);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_workItems.TryDequeue(out var workItem))
            {
                await Task.Delay(1000, stoppingToken);
            }
            else
            {
                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}