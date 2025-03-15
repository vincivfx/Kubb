using KubbAdminAPI.Models;

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
            
            await context.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(15 * 60000, stoppingToken);
        }

    }
}

