using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace KubbAdminAPI.Workers;

public class ScoreboardWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
        // throw new NotImplementedException();
        
        while (!stoppingToken.IsCancellationRequested)
        {

            var roundInit = DateTime.UtcNow;
            
            // TODO: update where conditions
            var challenges = context.Challenges.ToList();
            
            foreach (var challenge in challenges)
            {
                
                var teams = context.Teams.Where(team => team.Challenge == challenge).ToList();
                var answers = context.Answers.Where(answer => teams.Contains(answer.Team)).ToList();
                
                var scoreboard = Utils.ScoreboardGenerator.GenerateScoreboard(challenge, teams, answers);
                
                Console.WriteLine(scoreboard); 
                cache.Set(challenge.ChallengeId.ToString(), scoreboard);
                
                
            }

            var roundFinish = DateTime.UtcNow;
            
            var millis = Convert.ToInt32((roundFinish - roundInit).TotalMilliseconds);

            Console.WriteLine("scoreboard generation used " + millis + " millis");
            
            if (millis < 15000)
            {
                await Task.Delay(15000 - millis, stoppingToken);
            }
            
        }

    }
}