using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace KubbAdminAPI.Workers;

/**
 * MISSION: generate scoreboards for RUNNING challenges every 15 seconds
 */
public class ScoreboardWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

        var scoreboardRound = 1;

        while (!stoppingToken.IsCancellationRequested)
        {

            var roundInit = DateTime.UtcNow;

            var challenges = context.Challenges.Where(challenge => challenge.RunningStatus == Models.RunningChallengeStatus.Running).ToList();

            foreach (var challenge in challenges)
            {
                try
                {
                    var teams = context.Teams.Where(team => team.Challenge == challenge).ToList();
                    var answers = context.Answers.Where(answer => teams.Contains(answer.Team)).ToList();

                    var scoreboard = Utils.ScoreboardGenerator.GenerateScoreboard(challenge, teams, answers);

                    var cacheSettings = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                    };

                    cache.Set(challenge.ChallengeId.ToString(), scoreboard, cacheSettings);

                }
                catch (Exception exception)
                {
                    Console.WriteLine("ERROR GENERATING SCOREBOARD FOR " + challenge.Name + ": " + exception.Message + "\n");
                }
            }

            var roundFinish = DateTime.UtcNow;

            var millis = Convert.ToInt32((roundFinish - roundInit).TotalMilliseconds);

            Console.WriteLine("scoreboard round " + scoreboardRound.ToString() + ", generation used " + millis + " millis");

            if (millis < 15000)
            {
                await Task.Delay(15000 - millis, stoppingToken);
            }

            scoreboardRound += 1;

        }

    }
}