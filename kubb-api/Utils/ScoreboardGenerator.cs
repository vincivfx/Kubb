using KubbAdminAPI.Models;

namespace KubbAdminAPI.Utils;

public class ScoreboardGenerator
{
/*    
    public static string GenerateScoreboard(Challenge challenge, List<Team> teams, List<Answer> answers)
    {
        string output = "";
        var basePoints = 30;

        
        var teamPoints = new Dictionary<Team, List<ScoreboardCell>>();
        
        var rightAnswersGroups = new List<List<Answer>>(challenge.Questions.Count);
        
        foreach (var team in teams)
        {
            var singleTeamPoints = new List<ScoreboardCell>(challenge.Questions.Count);
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
            if (DateTime.UtcNow.AddSeconds(-45) < answer.Created) teamPoints[answer.Team][answer.Question].hightlight();

            if (!answer.AnswerText.Equals(challenge.Questions[answer.Question]))
            {
                points[answer.Question] += 1;
                teamPoints[answer.Team][answer.Question].error();
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
                var bonus = 0;
                if (i < bonuses.Count) bonus = bonuses[i];
                teamPoints[team][rightAnswersGroup[i].Question].correct(basePoints + Convert.ToInt32(diffTime) + bonus);
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
                if (t.getPoints() != null)
                    teamLine += t.getPoints() + (t.hightlight ? 'H' : '') + (t.isJolly ? 'J' : '') + (t.direction ? '+' : '-') + ",";
                else
                    teamLine += ",";
            }

            output += teamLine + "\n";


        }
        
        return output;
    }
}

public record ScoreboardCell {
    public int? points {get;set;}
    public bool highlight {get;set;}
    public bool? direction {get;set;}
    public bool isJolly {get;set;}

    public ScoreboardCell() {
        this.points = null;
        this.hightlight = false;
        this.direction = null;
        this.isJolly = false;
    }

    public void errorPoint(bool) {
        this.points ??= 0;
        this.points -= 10;
        if (this.direction == null) this.direction = false;
    }

    public void correct(int points) {
        this.points ??= 0;
        this.points += points;
        this.direction = true;
    }

    public void hightlight() {
        this.highlight = true;
    }

    public void setJolly() {
        this.isJolly = true;
    }

    public int? getPoints() {
        if (this.point == null) return null;
        if (this.isJolly) return this.point * 2;
        return this.points;
    }*/
}
