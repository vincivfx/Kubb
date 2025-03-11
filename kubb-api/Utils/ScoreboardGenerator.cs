using KubbAdminAPI.Models;

namespace KubbAdminAPI.Utils;

public class ScoreboardGenerator
{
    public static string GenerateScoreboard(Challenge challenge, List<Team> teams, List<Answer> answers)
    {
        string output = "";
        const int basePoints = 30;


        var teamPoints = new Dictionary<Guid, SimpleTeam>();

        var rightAnswersGroups = new List<List<Answer>>();
        for (int i = 0; i < challenge.Questions.Count; i++)
        {
            rightAnswersGroups.Add([]);
        }

        foreach (var team in teams)
        {
            var element = new SimpleTeam(challenge.Questions.Count, team);
            teamPoints.Add(team.TeamId, element);
        }

        var points = new List<int>();
        for (var i = 0; i < challenge.Questions.Count; i++) points.Add(basePoints);

        var deriva = teams.Count / 10 + 1;
        var bonuses = new List<int> { 20, 15, 10, 8, 6, 5, 4, 3, 2, 1 };
        var completeLineBonuses = new List<int> { 100, 60, 40, 30, 20, 10 };

        foreach (var answer in answers)
        {
            // security check on question index
            if (answer.Question >= challenge.Questions.Count) continue;
            if (DateTime.UtcNow.AddSeconds(-45) < answer.Created)
                teamPoints[answer.Team.TeamId].Cells[answer.Question].Highlight = true;

            if (!answer.AnswerText.Equals(challenge.Questions[answer.Question]))
            {
                points[answer.Question] += 1;
                teamPoints[answer.Team.TeamId].Cells[answer.Question].AddWrong();
            }
            else
            {
                // if a right question was given before, ignore the current one (problems with bonuses) 
                var validateAnswer = true;
                foreach (var rightAnswer in rightAnswersGroups[answer.Question])
                {
                    if (rightAnswer.Team == answer.Team)
                    {
                        validateAnswer = false;
                        break;
                    }
                }
                if (validateAnswer) rightAnswersGroups[answer.Question].Add(answer);
            }
        }

        foreach (var rightAnswersGroup in rightAnswersGroups)
        {
            rightAnswersGroup.Sort((a, b) => a.Created < b.Created ? -1 : 1);
        }

        for (var groupId = 0; groupId < points.Count; groupId += 1)
        {
            var rightAnswersGroup = rightAnswersGroups[groupId];

            double diffTime = 0;
            if (rightAnswersGroup.Count >= deriva)
            {
                diffTime = rightAnswersGroup[deriva - 1].Created.Subtract(challenge.StartTime!.Value).TotalMinutes;
            }
            else if (DateTime.UtcNow < challenge.EndTime)
            {
                diffTime = DateTime.UtcNow.Subtract(challenge.StartTime!.Value).TotalMinutes;
            }
            else
            {
                diffTime = challenge.EndTime!.Value.AddMinutes(-20).Subtract(challenge.StartTime!.Value).TotalMinutes;
            }

            points[groupId] += Convert.ToInt32(diffTime);

            for (var i = 0; i < rightAnswersGroup.Count; i++)
            {
                var team = rightAnswersGroup[i].Team.TeamId;
                var bonus = 0;
                if (i < bonuses.Count) bonus = bonuses[i];
                teamPoints[team].Cells[rightAnswersGroup[i].Question]
                    .AddRight(points[groupId] + bonus);
            }
        }

        var pointLine = "";
        for (var pointId = 0; pointId < points.Count; pointId++)
        {
            if (pointId != 0) pointLine += ",";
            pointLine += points[pointId];
        }

        output += pointLine + "\n";

        foreach (var teamPoint in teamPoints.ToList())
        {
            var teamLine = teamPoint.Value.TeamName + ",";

            for (var cell = 0; cell < teamPoint.Value.Cells.Count; cell += 1)
            {
                var t = teamPoint.Value.Cells[cell];
                if (cell > 0) teamLine += ",";
                if (t.HasBeenSet)
                    teamLine += t.GetPoints().ToString() + (t.Highlight ? 'H' : null) + (t.IsJolly ? 'J' : null) +
                                (t.Direction ? '+' : '-');
            }

            output += teamLine + "\n";
        }

        return output;
    }
}

public record ScoreboardCell
{
    private int Points { get; set; } = 0;
    public bool HasBeenSet { get; private set; } = false;
    public bool Highlight { get; set; } = false;
    public bool Direction { get; private set; } = false;
    public bool IsJolly { get; set; } = false;

    public void AddWrong()
    {
        Points -= 10;
        HasBeenSet = true;
    }

    public void AddRight(int rightAnswerPoints)
    {
        if (Direction) return;
        Points += rightAnswerPoints;
        Direction = true;
        HasBeenSet = true;
    }

    public int? GetPoints()
    {
        if (this.IsJolly) return this.Points * 2;
        return this.Points;
    }
}

public class SimpleTeam
{
    public SimpleTeam(int size, Team team)
    {
        TeamName = team.TeamName;
        TeamId = team.TeamId;
        for (var i = 0; i < size; i += 1) Cells.Add(new ScoreboardCell());
    }

    public List<ScoreboardCell> Cells { get; set; } = [];
    public string TeamName { get; set; }
    public Guid TeamId { get; set; }
}