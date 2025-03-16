using KubbAdminAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI.Workers;

/**
 * Cleaner worker deletes all logins expired every 15 minutes on the database
 */
public class CleanerWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var logins = context.Logins.Where(login => login.Expiration < DateTime.UtcNow);
            context.Logins.RemoveRange(logins);

            // Removing users registered but not yet verified in 1h
            var users = context.Users.Where(user => (user.Status & UserStatus.NeedsVerification) != 0 && user.Created.AddHours(1) < DateTime.UtcNow).ToList();
            context.Users.RemoveRange(users);

            // move challenges starting in the next 15 minutes (automatically) to 'running'
            var challengesToStart = context.Challenges.Where(challenge => 
                    challenge.StartTime < DateTime.UtcNow.AddMinutes(15) && 
                    (challenge.Status & ChallengeStatus.StartEnabled) != 0 &&
                    challenge.RunningStatus == RunningChallengeStatus.Submitted
            ).ToList();
            foreach (var challenge in challengesToStart)
            {
                challenge.RunningStatus = RunningChallengeStatus.Running;
            }


            // move finished challenges to 'frozen' runningStatus after (at least) 60 minutes from the endTime
            var challengesToFreeze = context.Challenges.Where(challenge => challenge.RunningStatus == RunningChallengeStatus.Submitted && challenge.EndTime!.Value.AddHours(1) < DateTime.UtcNow);
            foreach (var challenge in challengesToFreeze)
            {
                challenge.RunningStatus = RunningChallengeStatus.Frozen;
            }

            // TODO: move frozen challenges to 'terminated' runningStatus after one day from the endTime
            // saving everything in a file


            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(15 * 60000, stoppingToken);
        }

    }
}

