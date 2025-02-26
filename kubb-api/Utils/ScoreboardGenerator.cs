using KubbAdminAPI.Models;

namespace KubbAdminAPI.Utils;

public class ScoreboardGenerator
{
    
    public static string GenerateScoreboard(Challenge challenge, List<Team> teams, List<Answer> answers)
    {
        string output = "";
        var basePoints = 30;

        
        var teamPoints = new Dictionary<Team, List<int?>>();
        var rightAnswersGroups = new List<List<Answer>>(challenge.Questions.Count);
        
        foreach (var team in teams)
        {
            var singleTeamPoints = new List<int?>(challenge.Questions.Count);
            for (var i = 0; i < challenge.Questions.Count; i++) singleTeamPoints[i] = null;
            teamPoints.Add(team, singleTeamPoints);
        }
        
        var points = new List<int>(challenge.Questions.Count);
        
        var deriva = teams.Count / 10 + 1;
        var bonuses = new List<int>{20, 15, 10, 8, 6, 5, 4, 3, 2, 1};
        var completeLineBonuses = new List<int>{100, 60, 40, 30, 20, 10};

        foreach (var answer in answers)
        {
            // security check on question index
            if (answer.Question >= challenge.Questions.Count) continue;
            
            if (!answer.AnswerText.Equals(challenge.Questions[answer.Question]))
            {
                points[answer.Question] += 1;
                teamPoints[answer.Team][answer.Question] ??= 0;
                teamPoints[answer.Team][answer.Question] -= 10;
            }
            else
            {
                rightAnswersGroups[answer.Question].Add(answer);
            }
        }

        foreach (var rightAnswersGroup in rightAnswersGroups)
        {
            rightAnswersGroup.Sort((a, b) => a.Created < b.Created ? -1 : 1 );
        }

        foreach (var rightAnswersGroup in rightAnswersGroups)
        {
            var diffTime = rightAnswersGroup.Count < deriva ? challenge.EndTime.AddMinutes(-20).Subtract(challenge.StartTime).TotalMinutes : rightAnswersGroup[deriva - 1].Created.Subtract(challenge.StartTime).TotalMinutes;

            for (var i = 0; i < rightAnswersGroup.Count; i++)
            {
                var team = rightAnswersGroup[i].Team;
                teamPoints[team][rightAnswersGroup[i].Question] ??= 0;
                if (i < bonuses.Count) teamPoints[team][rightAnswersGroup[i].Question] += bonuses[i];
                teamPoints[team][rightAnswersGroup[i].Question] += basePoints;
                teamPoints[team][rightAnswersGroup[i].Question] += Convert.ToInt32(diffTime);
            }
        }

        var pointLine = "";
        foreach (var point in points)
        {
            pointLine += point + ",";
        }
        output += pointLine + "\n";

        foreach (var teamPoint in teamPoints.ToList())
        {
            var teamLine = teamPoint.Key.TeamName + ",";

            foreach (var t in teamPoint.Value)
            {
                if (t != null)
                    teamLine += t + ",";
                else
                    teamLine += ",";
            }

            output += teamLine + "\n";


        }
        
        return output;
    }
}